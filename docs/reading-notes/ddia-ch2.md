# DDIA Chapter 2 — Defining Nonfunctional Requirements

> **Sách:** Designing Data-Intensive Applications, 2nd Edition — Martin Kleppmann  
> **Trang:** 50–89  
> **Hoàn thành:** Tuần 3, Tháng 4/2026  
> **Project áp dụng:** Multi-tenant Fulfillment Platform (.NET 8, PostgreSQL, RabbitMQ, AWS)

---

## Key Concepts

### 1. Latency vs. Response Time
Response time là thứ client thực sự nhìn thấy (bao gồm network, queuing delay).
Latency là thời gian request chờ được xử lý. Đo bằng **percentiles**, không phải average.

- **p50:** 50% request nhanh hơn con số này — median, không đại diện cho worst case
- **p95:** 95% request nhanh hơn — đây là con số quan trọng nhất cho production SLA
- **p99:** 99% request nhanh hơn — tail latency, ảnh hưởng trực tiếp đến user experience
- **Average che giấu outliers** — 1 request 10 giây kéo average lên nhưng không thay đổi p50

### 2. Reliability
Hệ thống tiếp tục hoạt động đúng dù có hardware fault, software error, hoặc human error.

- **Fault** ≠ **Failure:** Fault là 1 component lệch khỏi spec. Failure là toàn bộ hệ thống ngừng hoạt động
- **Fault-tolerant:** Hệ thống xử lý được fault trước khi nó trở thành failure
- **Hardware fault:** Disk crash, RAM lỗi, power outage — giải quyết bằng redundancy
- **Software fault:** Bug ẩn kích hoạt khi gặp input đặc biệt — khó predict hơn hardware fault
- **Human error:** Config sai — giải quyết bằng rollback mechanism và sandbox environment

### 3. Scalability
Khả năng xử lý increased load — nhưng load phải được định nghĩa cụ thể trước.

- **Load parameters:** Tùy hệ thống — requests/sec, DB read/write ratio, concurrent users, cache hit rate
- **Scale up (vertical):** Máy mạnh hơn — đơn giản nhưng có giới hạn và expensive
- **Scale out (horizontal):** Nhiều máy hơn — phức tạp hơn nhưng không giới hạn lý thuyết
- **Elastic scaling:** Tự động thêm/bớt resource theo load — phù hợp workload unpredictable

### 4. Maintainability
Ba nguyên tắc: **Operability** (dễ vận hành), **Simplicity** (dễ hiểu), **Evolvability** (dễ thay đổi).

- **Operability:** Team ops có thể monitor, deploy, rollback dễ dàng — health checks, good logging
- **Simplicity:** Giảm accidental complexity — không thêm abstraction khi chưa cần
- **Evolvability:** Dễ thay đổi khi requirement thay đổi — ADR giúp hiểu tại sao quyết định như vậy

### 5. SLA vs. SLO vs. SLI
- **SLI** (Service Level Indicator): Metric đo thực tế — p95 latency = 180ms
- **SLO** (Service Level Objective): Target nội bộ — p95 latency < 200ms
- **SLA** (Service Level Agreement): Cam kết với khách hàng — uptime 99.9%, penalty nếu vi phạm

---

## Apply vào Fulfillment Platform

### Concept 1 — Latency Percentiles → Performance Targets

Thay vì nói "API phải nhanh", định nghĩa cụ thể bằng percentiles cho từng endpoint:
> **Noisy neighbor risk:** VIP tenant có millions of orders có thể làm
> slow p99 của tenant nhỏ hơn. Giải pháp Phase A: pagination bắt buộc
> trên mọi list endpoint, index đúng trên `(tenant_id, created_at DESC)`.

| Endpoint | p50 Target | p95 Target | p99 Target | Ghi chú |
|----------|-----------|-----------|-----------|---------|
| `GET /orders/:id` | < 50ms | < 200ms | < 500ms | Index trên `(tenant_id, id)` |
| `POST /orders` | < 200ms | < 500ms | < 1s | Transaction + inventory check |
| `GET /orders` (list) | < 100ms | < 300ms | < 800ms | Pagination bắt buộc |
| `GET /health/ready` | < 20ms | < 50ms | < 100ms | DB ping only |
| `POST /shipments/webhook` | < 100ms | < 300ms | < 500ms | Async processing sau khi ack |

> Ghi vào `docs/ARCHITECTURE.md` — interviewer hỏi "bạn measure performance thế nào?" có ngay câu trả lời cụ thể.

**Tại sao không dùng average?**
Nếu 95% request xử lý trong 50ms nhưng 5% mất 5 giây — average có thể là 300ms, trông "ổn". Nhưng user gặp 5 giây đó sẽ bỏ đi. p95 mới là con số phản ánh thực tế.

### Concept 2 — Reliability → Failure Story Cần Có

Phase A cần thiết kế và document ít nhất 3 failure scenario:

**Scenario 1 — Background service crash khi generate shipping label:**

```
→ Message broker (RabbitMQ Phase B) tự re-deliver task
→ Consumer xử lý idempotent — không tạo duplicate label
→ Nếu chưa có RabbitMQ (Phase A): ghi failed job vào DB, retry bằng hosted service
```

**Scenario 2 — Inventory concurrent request:**
```
2 order cùng lúc muốn mua item cuối cùng trong kho
→ Optimistic locking với row_version
→ 1 thành công, 1 nhận 409 Conflict
→ Client retry hoặc báo hết hàng
```

**Scenario 3 — Deploy lỗi:**
```
GitHub Actions deploy image mới
→ Health check /health/ready fail 3 lần
→ Pipeline tự động rollback về image cũ
→ Slack notification "Deploy failed, rolled back to v1.2.3"
```

> Đây là nội dung của ADR-002 phần "Migration path" và README phần "Runbook".

### Concept 3 — Scalability → Load Parameters của Project

Định nghĩa load parameters cụ thể cho Phase A (single tenant benchmark):

- **Target:** 50 concurrent users, 100 requests/second
- **Read/write ratio:** 80% read (GET orders, inventory) / 20% write (POST orders, PATCH status)
- **Peak load:** Cuối tháng khi merchant chạy promotion — dự kiến 3x normal load
- **Data volume Phase A:** < 100k orders per tenant, < 10k SKUs per tenant

Với parameters này, single EC2 t3.medium + RDS db.t3.micro là đủ. Không cần scale out ở Phase A.

> Chạy load test với 50 concurrent request bằng `dotnet-load-test` hoặc `k6`, ghi kết quả vào `ARCHITECTURE.md` trước khi kết thúc Phase A.

### Concept 4 — Maintainability → 3 Nguyên Tắc Áp Dụng Ngay

**Operability:**
- Serilog structured logging với `CorrelationId`, `TenantId`, `UserId` trên mọi log
- Health check `/health/ready` và `/health/live` — phân biệt rõ 2 loại
- GitHub Actions CD có bước rollback tự động nếu health check fail

**Simplicity:**
- Không dùng DDD full-blown ở Phase A — chỉ dùng Entity + Value Object cơ bản
- Không dùng CQRS tách read/write DB — chỉ dùng MediatR để tách handler
- Mỗi khi muốn thêm abstraction mới, hỏi: "Vấn đề thật nào đang được giải?"

**Evolvability:**
- Mỗi quyết định kiến trúc có ADR — khi muốn thay đổi, đọc ADR để hiểu trade-offs
- Database migration dùng EF Core — có thể rollback từng migration
- Feature flag đơn giản qua config — tắt feature mà không cần redeploy

### Concept 5 — SLI/SLO/SLA → Định Nghĩa Trước Khi Viết Code

Phase A không có khách hàng thật nên không cần SLA. Nhưng cần định nghĩa SLO nội bộ:

| SLI | SLO Phase A | Đo bằng |
|-----|------------|---------|
| API availability | > 99% trong giờ làm việc | CloudWatch /health/ready |
| p95 latency orders endpoint | < 300ms | Serilog + CloudWatch metrics |
| Error rate | < 1% trên tổng request | Serilog error logs |
| Deploy success rate | > 90% | GitHub Actions success rate |

> Ghi SLO này vào `docs/ARCHITECTURE.md`. Khi deploy lên EC2, setup CloudWatch alarm khi vi phạm.

---

## Open Questions

- Khi có nhiều tenant với load pattern rất khác nhau (1 tenant 10k orders/ngày, tenant khác 10 orders/ngày), shared infrastructure có bị "noisy neighbor" problem không? Giải quyết thế nào ở Phase A?
- p99 latency của `POST /orders` có thể bị spike khi inventory reservation dùng pessimistic lock — có nên dùng optimistic lock thay không? Trade-off là gì?

---

## Liên kết ADR

| ADR | Quyết định | Concept liên quan |
|-----|-----------|-------------------|
| [ADR-001](../adr/ADR-001-postgresql.md) | Chọn PostgreSQL | Scalability — index strategy |
| [ADR-002](../adr/ADR-002-multitenancy.md) | Shared schema | Reliability — tenant isolation |
| [ADR-003](../adr/ADR-003-ef-dapper.md) | EF Core + Dapper | Performance — p95 latency |
| [ADR-005](../adr/ADR-005-auth.md) | JWT Auth | Operability — stateless |

---

*Tiếp theo: [DDIA Ch.3 — Data Models and Query Languages](./ddia-ch3.md)*



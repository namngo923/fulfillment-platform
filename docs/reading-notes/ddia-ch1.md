# DDIA Chapter 1 — Trade-Offs in Data Systems Architecture

> **Sách:** Designing Data-Intensive Applications, 2nd Edition — Martin Kleppmann  
> **Trang:** 25–49  
> **Hoàn thành:** Tuần 1, Tháng 1/2026  
> **Project áp dụng:** Multi-tenant Fulfillment Platform (.NET 8, PostgreSQL, RabbitMQ, AWS)

---

## Key Concepts

### 1. There are no solutions; there are only trade-offs

Không có kiến trúc nào là "tốt nhất" — chỉ có sự đánh đổi phù hợp với context.  
Câu hỏi đúng không phải *"cái nào tốt hơn?"* mà là *"trade-off của mỗi lựa chọn là gì?"*

### 2. Operational (OLTP) vs. Analytical (OLAP)

Hệ thống tối ưu cho OLTP (point queries, low-latency, individual transactions) thường
hoạt động kém khi phải aggregate hàng triệu records cho business reports.

- **OLTP:** Tối ưu cho read/write từng record — index trên primary key, transaction nhỏ
- **OLAP:** Tối ưu cho scan toàn bộ dataset — columnar storage, batch aggregation

### 3. Cloud vs. Self-Hosting

Managed cloud services (AWS RDS, S3) mang lại elasticity và giảm operational overhead,
nhưng đánh đổi bằng reduced control và vendor lock-in.

### 4. Distributed vs. Single-Node

Distributed systems cung cấp fault tolerance và scalability, nhưng sinh ra các failure modes
mới: unreliable networks, clock skew, partial failures, và data consistency issues.

---

## Apply vào Fulfillment Platform

### Concept 1 — Trade-offs

Mọi quyết định kiến trúc trong project đều là trade-off có chủ đích, không phải đáp án đúng
tuyệt đối. Ví dụ điển hình ngay Phase 1:

**Shared schema multi-tenancy** (tất cả tenant chung 1 database, phân tách bằng `tenant_id`):
- ✅ Simple to build, easy to maintain, single migration
- ❌ Reduced isolation — 1 query thiếu `WHERE tenant_id = ?` là lộ data tenant khác
- ❌ 1 tenant query nặng có thể ảnh hưởng performance tenant khác

> Ghi nhận trong **ADR-002:** *"We chose shared schema over separate schemas, accepting
> reduced isolation in exchange for operational simplicity at this stage."*

### Concept 2 — OLTP vs. OLAP

Project có **cả hai loại query** ngay từ Phase 1 — đây là lý do dùng EF Core + Dapper song song.

| Query type | Ví dụ trong project | Tool |
|---|---|---|
| OLTP | `GET /orders/:id` — lấy 1 đơn theo ID | EF Core |
| OLTP | `POST /orders` — tạo đơn, update inventory | EF Core |
| OLAP-lite | Dashboard: tổng doanh thu 30 ngày | Dapper + raw SQL |
| OLAP-lite | Report: top 10 SKU bán chạy trong tháng | Dapper + raw SQL |

Dùng EF Core cho report query sẽ load toàn bộ records về memory rồi mới aggregate — kém
hiệu quả. Dapper cho phép viết thẳng SQL tối ưu:

```sql
SELECT DATE(created_at), SUM(total_amount)
FROM orders
WHERE tenant_id = @tenantId
  AND created_at >= NOW() - INTERVAL '30 days'
GROUP BY DATE(created_at)
ORDER BY 1
```

> Ghi nhận trong **ADR-003:** *"EF Core for write + simple read, Dapper for complex
> analytical queries — use the right tool for the right job."*

### Concept 3 — Cloud vs. Self-Hosting

Project chọn **managed AWS** — trade-off có chủ đích:

| | Managed AWS | Self-host |
|---|---|---|
| **Nhận được** | Không ops OS/patching/backup, RDS tự failover | Full control, portable, không vendor lock-in |
| **Đánh đổi** | Vendor lock-in một phần, chi phí cao hơn khi scale | Phải tự quản lý uptime, disk, memory |

Với quỹ thời gian 14h/tuần, không thể vừa học code vừa quản lý infrastructure thủ công
→ managed cloud là lựa chọn đúng **ở giai đoạn này**.

**Quyết định cụ thể Phase 3 — RabbitMQ vs AWS SQS:**
- SQS = managed, không ops, nhưng lock-in và API khác RabbitMQ
- RabbitMQ tự host trên EC2 = portable, học được message broker thật sự
- **Quyết định:** Giữ RabbitMQ tự host — mục tiêu là học, không phải minimize ops

### Concept 4 — Distributed vs. Single-Node

Đây là lý do quan trọng nhất để **bắt đầu bằng monolith** (Phase 1) thay vì microservices.

Single-node monolith tránh toàn bộ failure modes Kleppmann cảnh báo:
- Không network call giữa services → không partial failure
- Không data consistency issue giữa nhiều databases
- Không phức tạp distributed transaction

**Failure mode sẽ gặp khi tách service ở Phase 2:**

```
1. Order Service gọi Inventory Service reserve stock
2. Inventory reserve thành công
3. Order Service crash trước khi save order
4. → Stock bị reserve nhưng không có order
5. → Inventory leak
```

Đây là bài toán distributed transaction kinh điển — sẽ được giải thích ở Ch.8 và Ch.9.
Cách handle: Outbox Pattern với MassTransit (Phase 2).

---

## Open Questions

- Khi nào thì Fulfillment Platform cần tách OLAP workload ra dedicated reporting database?
  Ở scale nào thì Dapper + raw SQL không còn đủ nữa?
- Với shared schema multi-tenancy, nếu 1 tenant có data volume gấp 100x các tenant khác
  thì xử lý thế nào? Tách schema riêng hay partition table?

---

## Liên kết ADR

| ADR | Quyết định | Concept liên quan |
|-----|-----------|-------------------|
| [ADR-001](../adr/ADR-001-postgresql.md) | Chọn PostgreSQL | Trade-offs |
| [ADR-002](../adr/ADR-002-multitenancy.md) | Shared schema multi-tenancy | Trade-offs |
| [ADR-003](../adr/ADR-003-ef-dapper.md) | EF Core + Dapper | OLTP vs OLAP |
| [ADR-006](../adr/ADR-006-monolith-first.md) | Monolith trước microservices | Distributed vs Single-Node |

---

*Tiếp theo: [DDIA Ch.2 — Defining Nonfunctional Requirements](./ddia-ch2.md)*



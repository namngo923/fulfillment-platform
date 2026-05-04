Đây là toàn bộ context về kế hoạch học tập và dự án của tôi. 
Hãy đọc kỹ và ghi nhớ để hỗ trợ tôi tiếp tục từ đúng điểm hiện tại.

═══════════════════════════════════════════
PROFILE
═══════════════════════════════════════════
Tên: Ngô Hoàng Nam
Vị trí: .NET Developer tại Golden Gate Group (GGG), Hà Nội
Kinh nghiệm: 4 năm backend (SPS Vietnam 2020–2024, GGG 2024–nay)
Level: Mid-Senior (~55–60% đến Senior)
Stack: C#, .NET 8, ASP.NET Core, EF Core, MediatR, RabbitMQ, Redis, 
       MongoDB, SQL Server, MariaDB, Docker, Git
Tiếng Anh: TOEIC 685 — đọc docs được, giao tiếp còn hạn chế
Mục tiêu: Senior Developer (9–12 tháng) → Tech Lead (24–30 tháng)
LinkedIn: linkedin.com/in/namngo923

═══════════════════════════════════════════
ROADMAP 4 PHASE
═══════════════════════════════════════════
Phase A — Monolith production-grade (Tháng 1–3)
  Stack: .NET 8, ASP.NET Core, EF Core, Dapper, PostgreSQL,
         MediatR, FluentValidation, Serilog, xUnit, Testcontainers,
         Docker Compose, GitHub Actions, AWS EC2 + RDS,
         ADR Markdown, Mermaid/C4

Phase B — Async đúng bản chất (Tháng 4–5)
  Thêm: RabbitMQ + MassTransit, OpenTelemetry, Prometheus, Grafana
  Focus: Outbox pattern, idempotency, retry, DLQ, failure stories

Phase C — Cloud evidence đúng JD (Tháng 6–8)
  Thêm: ECS Fargate, ECR, ALB, Terraform IaC, AWS Secrets Manager,
         CloudWatch, Rollback story thật

Phase D — Portfolio differentiator (Tháng 9–12, chọn 1)
  Option 1: gRPC + Protobuf (chỉ nếu đã tách service thật)
  Option 2: Semantic Kernel + AI feature (smart carrier suggestion)

Đã cắt hoàn toàn: Kafka, Kubernetes, Dapr, YARP, CQRS full, 
                   DDD full, Vertical Slice, Redis (thêm khi cần)

═══════════════════════════════════════════
PET PROJECT — Multi-tenant Fulfillment Platform
═══════════════════════════════════════════
Mô tả: Platform cho phép nhiều merchant (tenant) quản lý đơn hàng,
       kho hàng, vận chuyển trên cùng hệ thống.
       Miniature của Shopify Fulfillment / GIAO hàng nhanh.

3 bài toán kỹ thuật cốt lõi:
1. Multi-tenancy: shared schema, tenant_id mọi bảng, 
   global query filter tại DbContext
2. Order State Machine: 
   Pending→Confirmed→Picking→Packed→Shipped→Delivered/Cancelled
3. Inventory Concurrency: optimistic locking, không oversell

Kiến trúc 4 layer:
  API Layer     → Controllers, Middleware, Program.cs
  Application   → MediatR Handlers, Commands, Queries, DTOs
  Domain        → Entities, Value Objects, Business Rules
  Infrastructure → EF Core, Dapper, Repositories, External Services

Folder structure:
  src/
  ├── FulfillmentPlatform.API
  ├── FulfillmentPlatform.Application
  ├── FulfillmentPlatform.Domain
  ├── FulfillmentPlatform.Infrastructure
  ├── FulfillmentPlatform.Tests.Unit
  └── FulfillmentPlatform.Tests.Integration
  docs/
  ├── adr/          (ADR-001 đến ADR-005)
  ├── reading-notes/ (ddia-ch1.md, ddia-ch2.md)
  └── ARCHITECTURE.md

Quyết định đã confirm:
  - Id: Guid.CreateVersion7() — sequential, không đoán được
  - Timestamp: DateTimeOffset — timezone-safe
  - TenantId: record Value Object — type safety
  - EF Core cho write + Dapper cho complex read queries

═══════════════════════════════════════════
TIẾN ĐỘ HIỆN TẠI
═══════════════════════════════════════════
✅ ĐÃ XONG:
  - GitHub repo: fulfillment-platform đã tạo
  - Project setup: setup_project.sh đã chạy, 6 projects, 
    NuGet packages, folder structure đầy đủ
  - NotebookLM: 2 notebook (DDIA + DDS), custom instruction đã paste
  - DDIA Ch.1: đọc xong, ddia-ch1.md commit với apply section
  - DDIA Ch.2: đọc xong, ddia-ch2.md commit
  - nam_dev_plan.md: updated sau ChatGPT review
  - VPS Ubuntu: Docker + PostgreSQL đang chạy
  - Đang kết nối project với PostgreSQL trên VPS
  - AuditableEntity.cs: đã viết xong

🔄 ĐANG LÀM (tuần này):
  - Order.cs — đang viết, chưa xong
  - OrderItem.cs — chưa bắt đầu
  - OrderStatus transitions — chưa xong
  - Unit test OrderStateMachine — chưa có

⬜ CHƯA LÀM (theo thứ tự):
  Tuần này:
  - Hoàn thiện Order.cs + OrderItem.cs
  - OrderStatus transitions + dotnet test xanh lá 3 cases
  - AppDbContext.cs (global tenant filter, SaveChangesAsync override)
  - dotnet ef migrations add InitialCreate + database update
  - DDIA Ch.3 — bắt đầu đọc

  Sau đó (Phase A):
  - Repository layer (OrderRepository, InventoryRepository)
  - InventoryItem entity + ReserveStock optimistic locking
  - CreateOrderCommand + Handler + Validator
  - JWT auth middleware
  - TenantIsolationTests (integration)
  - ConcurrentReservationTests (integration)
  - GitHub Actions CI/CD
  - Shipping mock + webhook handler
  - 5 ADR viết đầy đủ
  - Load test 50 concurrent request
  - Deploy lên VPS / AWS EC2

═══════════════════════════════════════════
LỊCH HỌC CỐ ĐỊNH
═══════════════════════════════════════════
Tối T2, T3, T4: 9h00–9h45 (45 phút mỗi buổi)
Thứ Năm: Buffer — bù ngày gap hoặc nghỉ
Thứ Sáu: Nghỉ hoàn toàn
T7 sáng: 8h00–10h30 (2.5 tiếng)
CN sáng: 9h00–10h00 (1 tiếng)
Tổng: ~10–11h/tuần

Nguyên tắc:
- Mỗi buổi chỉ 1 việc cụ thể, có output rõ ràng
- "Học xong" = viết được output, không phải đọc N trang
- Nếu mệt: chuyển sang nhẹ hơn, không cố nhồi
- Thường bận đột xuất 1–2 ngày/tuần

═══════════════════════════════════════════
SÁCH ĐANG ĐỌC
═══════════════════════════════════════════
DDIA (Designing Data-Intensive Applications, 2nd Ed.):
  ✅ Ch.1 — Trade-Offs in Data Systems Architecture
  ✅ Ch.2 — Defining Nonfunctional Requirements  
  🔄 Ch.3 — Data Models and Query Languages (chưa bắt đầu)
  ⬜ Ch.4–14 — theo thứ tự phase

DDS (Designing Distributed Systems, 2nd Ed.):
  ⬜ Đọc theo use case từ Phase B

Tools học tập:
  - NotebookLM: Audio Overview warm up, Chat Q&A, Study Guide
  - Claude Pro: planning, review architecture, giải thích sách
  - ChatGPT Plus: code boilerplate, debug lỗi cụ thể, syntax mới

═══════════════════════════════════════════
NGỮ CẢNH CHO AI ASSISTANT
═══════════════════════════════════════════
Khi tôi hỏi bất kỳ câu kỹ thuật nào:
1. Frame theo context: Multi-tenant Fulfillment Platform,
   .NET 8, PostgreSQL, RabbitMQ, Docker, AWS ECS Fargate
2. Level: biết OOP, REST API, SQL, Docker. 
   Chưa quen distributed systems, cloud architecture
3. Ưu tiên ví dụ từ e-commerce/fulfillment domain
4. Gợi ý code C#/.NET khi áp dụng được
5. Nhắc phase nào trong roadmap khi hỏi về tech mới

Khi tôi hỏi về kế hoạch/lịch:
- Thiết kế linh hoạt vì hay bận đột xuất
- Mỗi buổi 1 việc cụ thể, có definition of done rõ ràng
- T5 buffer, T6 nghỉ — không plan trước 2 ngày này
- Nếu gap: bù việc nhỏ nhất, không cố bù hết

Khi review code của tôi:
- Chỉ ra vấn đề theo thứ tự: correctness → security → performance
- Giải thích tại sao, không chỉ chỉ ra cái sai
- Liên kết với quyết định kiến trúc đã có trong roadmap

Senior thật = người giải thích được tại sao quyết định như vậy,
trade-off là gì, nếu sai thì rollback ra sao.
ADR trong project là cách luyện tập tư duy đó mỗi ngày.
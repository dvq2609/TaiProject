using Microsoft.EntityFrameworkCore;

namespace BE.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;    
        public DbSet<Product> Products { get; set; } = null!;   
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình Precision cho kiểu Decimal của Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(p => p.Star)
                    .HasColumnType("decimal(3,1)"); // Ví dụ: 4.5, 5.0
            });

            // 2. Thay đổi hành vi Cascade Delete sang Restrict để an toàn dữ liệu
            // Ngăn chặn xóa Role nếu có User đang thuộc Role đó
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ngăn chặn xóa Category nếu có Product đang thuộc Category đó
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. Ngăn chặn xóa User/Product và Cascade cho OrderItem
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. Seed dữ liệu mặc định cho bảng Role
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Quản trị viên hệ thống", Status = true },
                new Role { RoleId = 2, RoleName = "User", Description = "Người dùng thông thường", Status = true }
            );
        }
    }
}

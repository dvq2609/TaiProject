-- =========================================================================
-- VƯƠN - SQL Server Database Seed Data Script
-- Populates: Roles, Users, Categories, and Products (with frontend images)
-- Database name: vuon
-- =========================================================================

-- 1. Thêm cột Image vào bảng Product nếu chưa có (trong trường hợp Migration chưa cập nhật)
IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND name = 'Image'
)
BEGIN
    ALTER TABLE [dbo].[Product] ADD [Image] NVARCHAR(MAX) NULL;
END
GO

-- 2. Nhập dữ liệu mẫu cho bảng Role (Quyền)
SET IDENTITY_INSERT [dbo].[Role] ON;

MERGE INTO [dbo].[Role] AS Target
USING (VALUES 
    (1, N'Admin', N'Quản trị viên hệ thống', 1),
    (2, N'User', N'Người dùng thông thường', 1)
) AS Source (RoleId, RoleName, Description, Status)
ON Target.RoleId = Source.RoleId
WHEN MATCHED THEN
    UPDATE SET RoleName = Source.RoleName, Description = Source.Description, Status = Source.Status
WHEN NOT MATCHED BY TARGET THEN
    INSERT (RoleId, RoleName, Description, Status) 
    VALUES (Source.RoleId, Source.RoleName, Source.Description, Source.Status);

SET IDENTITY_INSERT [dbo].[Role] OFF;
GO

-- 3. Nhập dữ liệu mẫu cho bảng User (Người dùng)
-- Sử dụng BCrypt Hash cho mật khẩu mặc định "123456": $2a$12$R9h/lIPzNgbdy11QJbMc8eb42tHRLBVsa5AZYgKySJ1FMhmJZ.t2a
SET IDENTITY_INSERT [dbo].[User] ON;

MERGE INTO [dbo].[User] AS Target
USING (VALUES 
    (1, 1, N'Admin Vườn', N'admin@vuon.vn', N'0901234567', N'$2a$11$6StPx5/MJ3YStF79bd1yWeGsGcqJJre8EEchTNgtMbTQqiwYL9qFC', 1, GETDATE()),
    (2, 2, N'Nguyễn Văn A', N'nguyenvana@email.com', N'0912345678', N'$2a$11$6StPx5/MJ3YStF79bd1yWeGsGcqJJre8EEchTNgtMbTQqiwYL9qFC', 1, GETDATE())
) AS Source (UserId, RoleId, FullName, Email, PhoneNumber, PasswordHash, Status, CreatedAt)
ON Target.UserId = Source.UserId
WHEN MATCHED THEN
    UPDATE SET RoleId = Source.RoleId, FullName = Source.FullName, Email = Source.Email, PhoneNumber = Source.PhoneNumber, PasswordHash = Source.PasswordHash, Status = Source.Status
WHEN NOT MATCHED BY TARGET THEN
    INSERT (UserId, RoleId, FullName, Email, PhoneNumber, PasswordHash, Status, CreatedAt) 
    VALUES (Source.UserId, Source.RoleId, Source.FullName, Source.Email, Source.PhoneNumber, Source.PasswordHash, Source.Status, Source.CreatedAt);

SET IDENTITY_INSERT [dbo].[User] OFF;
GO

-- 4. Nhập dữ liệu mẫu cho bảng Category (Danh mục sản phẩm)
SET IDENTITY_INSERT [dbo].[Category] ON;

MERGE INTO [dbo].[Category] AS Target
USING (VALUES 
    (1, N'Cây rau'),
    (2, N'Cây hoa'),
    (3, N'Combo'),
    (4, N'Phụ kiện')
) AS Source (CategoryId, Name)
ON Target.CategoryId = Source.CategoryId
WHEN MATCHED THEN
    UPDATE SET Name = Source.Name
WHEN NOT MATCHED BY TARGET THEN
    INSERT (CategoryId, Name) VALUES (Source.CategoryId, Source.Name);

SET IDENTITY_INSERT [dbo].[Category] OFF;
GO

-- 5. Nhập dữ liệu mẫu cho bảng Product (Sản phẩm với ảnh thực tế từ HTML)
SET IDENTITY_INSERT [dbo].[Product] ON;

MERGE INTO [dbo].[Product] AS Target
USING (VALUES 
    (1, N'Rau cải xanh', 1, 120000.00, 50, 4.5, 1, 'https://tse1.mm.bing.net/th/id/OIP.YMyyO4B2E7JowCCN-rgPEwHaFj?rs=1&pid=ImgDetMain&o=7&rm=3'),
    (2, N'Cà chua bi', 1, 120000.00, 30, 4.8, 1, 'https://images.unsplash.com/photo-1592841200221-a6898f307baa?w=400'),
    (3, N'Hoa hồng', 2, 120000.00, 20, 4.9, 1, 'https://img.thuthuatphanmem.vn/uploads/2018/09/24/hinh-anh-hoa-hong-dep-nhat_053955504.jpg'),
    (4, N'Hoa oải hương', 2, 120000.00, 45, 4.6, 1, 'https://charsawfarms.com/cdn/shop/files/PurpleBouquetlavender2.jpg?v=1710207668&width=1946'),
    (5, N'Combo cây rau gia vị', 3, 120000.00, 25, 4.7, 1, 'https://images.unsplash.com/photo-1466692476868-aef1dfb1e735?w=400'),
    (6, N'Phân bón hữu cơ', 4, 10000.00, 100, 4.4, 1, 'https://th.bing.com/th/id/R.6c286dba498a4c368b9da7b62e2e04a6?rik=HGOrgHSmda8Yqg&pid=ImgRaw&r=0'),
    (7, N'Rau diếp xanh', 1, 120000.00, 60, 4.3, 1, 'https://images.unsplash.com/photo-1622206151226-18ca2c9ab4a1?w=400'),
    (8, N'Hoa cúc vàng', 2, 120000.00, 35, 4.5, 1, 'https://media.chuabavang.com/files/tu_chinh/2021/12/28/hoa-cuc-vang-clb-cuc-vang-chua-ba-vang-0839.jpg'),
    (9, N'Rau húng quế', 1, 80000.00, 80, 4.6, 1, 'https://images.unsplash.com/photo-1618375569909-3c8616cf7733?w=400'),
    (10, N'Chậu đất nung', 4, 45000.00, 150, 4.2, 1, 'https://images.unsplash.com/photo-1622372738946-62e02505feb3?w=400'),
    (11, N'Combo kit trồng rau', 3, 250000.00, 40, 4.9, 1, 'https://images.unsplash.com/photo-1416879595882-3373a0480b5b?w=400'),
    (12, N'Hoa dã yên thảo', 2, 95000.00, 28, 4.4, 1, 'https://images.unsplash.com/photo-1490750967868-88df5691cc9e?w=400')
) AS Source (ProductId, Name, CategoryId, Price, Stock, Star, Status, Image)
ON Target.ProductId = Source.ProductId
WHEN MATCHED THEN
    UPDATE SET Name = Source.Name, CategoryId = Source.CategoryId, Price = Source.Price, Stock = Source.Stock, InStock = 1, Star = Source.Star, Status = Source.Status, Image = Source.Image
WHEN NOT MATCHED BY TARGET THEN
    INSERT (ProductId, Name, CategoryId, Price, Stock, InStock, Star, Status, Image) 
    VALUES (Source.ProductId, Source.Name, Source.CategoryId, Source.Price, Source.Stock, 1, Source.Star, Source.Status, Source.Image);

SET IDENTITY_INSERT [dbo].[Product] OFF;
GO

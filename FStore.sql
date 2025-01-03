USE [master]
GO
/****** Object:  Database [FStore]    Script Date: 3/4/2024 12:54:19 PM ******/
CREATE DATABASE [FStore]

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [FStore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FStore] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FStore] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FStore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FStore] SET RECOVERY FULL 
GO
ALTER DATABASE [FStore] SET  MULTI_USER 
GO
ALTER DATABASE [FStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FStore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FStore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FStore] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FStore] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FStore] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FStore', N'ON'
GO
ALTER DATABASE [FStore] SET QUERY_STORE = OFF
GO
USE [FStore]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[account_id] [int] IDENTITY(1,1) NOT NULL,
	[full_name] [varchar](100) NOT NULL,
	[mail] [varchar](100) NOT NULL,
	[address] [varchar](100) NOT NULL,
	[dob] [date] NULL,
	[gender] [bit] NULL,
	[phone] [varchar](10) NULL,
	[password] [varchar](100) NULL,
	[active] [bit] NOT NULL,
	[role_id] [int] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [varchar](100) NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[coupon_id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](100) NOT NULL,
	[discount_percent] [int] NOT NULL,
	[expiration_date] [date] NULL,
	[is_deleted] [bit] NULL,
 CONSTRAINT [PK__Coupons__58CF6389DA94C769] PRIMARY KEY CLUSTERED 
(
	[coupon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[image_link] [varchar](100) NULL,
	[create_date] [date] NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order_details]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_details](
	[order_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NOT NULL,
	[address] [varchar](100) NOT NULL,
	[create_date] [date] NULL,
	[shipping_date] [date] NULL,
	[required_date] [date] NULL,
	[status] [int] NULL,
	[payment_id] [int] NULL,
	[coupon_id] [int] NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[payment_name] [varchar](100) NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[product_id] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [varchar](100) NOT NULL,
	[price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[description] [varchar](max) NULL,
	[category_id] [int] NOT NULL,
	[image_id] [int] NOT NULL,
	[create_date] [date] NULL,
	[create_by] [int] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 3/4/2024 12:54:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](100) NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (4, N'Admin', N'sinhdqhe150849@fpt.edu.vn', N'Hung Yen', CAST(N'2001-03-14' AS Date), 1, N'0917076864', N'45310317335BD2615A915FC23D781BA7', 1, 1, 0)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (6, N'Staff 1', N'sinhdq@fpt.com', N'Ha Noi', CAST(N'2001-03-14' AS Date), 1, N'0917076864', N'45310317335BD2615A915FC23D781BA7', 1, 2, 0)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (7, N'Nguyen Van B', N'nguyenvanb@gmail.com', N'Ha Nam', CAST(N'2024-01-01' AS Date), 1, N'0985054398', N'62A2EFC2962A3BB876555A73212DA122', 1, 3, 0)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (8, N'Pham Van K', N'phamvank@gmail.com', N'Ha Noi', CAST(N'2024-01-04' AS Date), 1, N'0917076864', N'CF814721358D09942B255746542AD2A4', 1, 2, 1)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (1003, N'Dang Quoc Sinh', N'sinhphuung2001@gmail.com', N'Hung Yen', CAST(N'2001-03-14' AS Date), 1, N'0917076864', N'45310317335BD2615A915FC23D781BA7', 1, 3, 0)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (1, N'Samsung', 0)
INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (2, N'Apple', 0)
INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (3, N'Xiaomi', 0)
INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (1002, N'Nokia', 0)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Coupons] ON 

INSERT [dbo].[Coupons] ([coupon_id], [code], [discount_percent], [expiration_date], [is_deleted]) VALUES (5, N'NORMAL', 0, NULL, 0)
INSERT [dbo].[Coupons] ([coupon_id], [code], [discount_percent], [expiration_date], [is_deleted]) VALUES (6, N'KM2024', 15, CAST(N'2024-03-29' AS Date), 0)
INSERT [dbo].[Coupons] ([coupon_id], [code], [discount_percent], [expiration_date], [is_deleted]) VALUES (7, N'KMT3', 10, CAST(N'2024-03-31' AS Date), 1)
SET IDENTITY_INSERT [dbo].[Coupons] OFF
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (1, N'samsungs24.jpg', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (2, N'samsungs22.jpg', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (3, N'samsungzfold5.jpg', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (4, N'Iphone15prm.png', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (7, N'432532.png', CAST(N'2024-01-19' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (8, N'Presentation1.png', CAST(N'2024-01-19' AS Date), 0)
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
INSERT [dbo].[Order_details] ([order_id], [product_id], [quantity], [is_deleted]) VALUES (1, 2, 1, 0)
INSERT [dbo].[Order_details] ([order_id], [product_id], [quantity], [is_deleted]) VALUES (20, 3, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([order_id], [customer_id], [address], [create_date], [shipping_date], [required_date], [status], [payment_id], [coupon_id], [is_deleted]) VALUES (1, 1003, N'Hung Yen', CAST(N'2024-02-14' AS Date), CAST(N'2024-03-04' AS Date), CAST(N'2024-03-04' AS Date), 3, 2, 5, 0)
INSERT [dbo].[Orders] ([order_id], [customer_id], [address], [create_date], [shipping_date], [required_date], [status], [payment_id], [coupon_id], [is_deleted]) VALUES (20, 1003, N'Hung Yen', CAST(N'2024-03-04' AS Date), NULL, NULL, 1, 2, 5, 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (1, N'PAYMENT ONLINE', 0)
INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (2, N'COD', 0)
INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (1002, N'Zalo pay', 1)
INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (1003, N'Zalo pay', 1)
INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (1004, N'Zalo pay', 1)
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (2, N'Galaxy S24 Ultra', 33990000, 194, N'With the connectivity power of the Samsung Galaxy S24 Ultra, you can connect with the world easier than ever, overcoming all language barriers when communicating. Not only normal translation, S24 Ultra also has the ability to directly translate two-way calls in real time. You don''t even need to be connected to the Internet to understand what the other person is saying and vice versa. This feature makes it possible to communicate across borders. Whether it''s a voice or text message, S24 Ultra will quickly translate into Vietnamese and send back the message in the language of the other side.', 1, 1, CAST(N'2024-01-24' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (3, N'Samsung Galaxy S22 5G 128GB', 12490000, 194, N'Samsung Galaxy S22 is a leap forward in video technology in the mobile generation. At the same time, the phone also opens up a series of today''s leading breakthrough innovations, from the "flattering" flat beveled screen to the first advanced 4nm processor on the Samsung smartphone generation.', 1, 2, CAST(N'2024-01-30' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (4, N'Samsung Galaxy Z Fold5 5G', 31790000, 200, N'Samsung Galaxy Z Fold5 affirms its position as a pioneering folding phone, boldly breaking through old boundaries, leading with advanced Flex hinge technology. The device also opens up the most comprehensive range of smart experiences through a large screen, outstanding performance, S-Pen compatibility, allowing optimal multitasking, speeding up work effectively, giving users confidence. Connect flexibly with Galaxy Z Fold 5.', 1, 3, CAST(N'2024-01-18' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (5, N'iPhone 15 Pro Max 256GB', 32690000, 200, N'iPhone 15 Pro Max is the most advanced iPhone with the largest screen, best battery life, strongest configuration and super durable, super light aerospace-standard Titanium frame design. iPhone 15 Pro Max possesses Apple''s most outstanding features. Accordingly, users will experience a high-end iPhone with "huge" performance with A17 Pro chip, titanium frame, upgraded zoom capabilities, new action buttons,...', 2, 4, CAST(N'2024-01-30' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (6, N'MacBook Air 13 inch M1 2020', 18000000, 200, N'The MacBook Air with the most groundbreaking performance ever is here. The all-new Apple M1 processor brings the power of the 2020 13-inch MacBook Air M1 beyond user expectations, able to run heavy tasks and amazing battery life.', 2, 7, CAST(N'2024-01-19' AS Date), 4, 1)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (1002, N'ss15', 20000, 20000, N'ádfgnm', 1, 7, CAST(N'2024-01-23' AS Date), 4, 1)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (1, N'Admin', 0)
INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (2, N'Staff', 0)
INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (3, N'User', 0)
INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (1002, N'Guest', 0)
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [fk_account_role] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [fk_account_role]
GO
ALTER TABLE [dbo].[Order_details]  WITH CHECK ADD  CONSTRAINT [fk_item_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO
ALTER TABLE [dbo].[Order_details] CHECK CONSTRAINT [fk_item_product]
GO
ALTER TABLE [dbo].[Order_details]  WITH CHECK ADD  CONSTRAINT [fk_oder_items] FOREIGN KEY([order_id])
REFERENCES [dbo].[Orders] ([order_id])
GO
ALTER TABLE [dbo].[Order_details] CHECK CONSTRAINT [fk_oder_items]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_coupon] FOREIGN KEY([coupon_id])
REFERENCES [dbo].[Coupons] ([coupon_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_coupon]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_customer]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_payment] FOREIGN KEY([payment_id])
REFERENCES [dbo].[Payment] ([payment_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_payment]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_account] FOREIGN KEY([create_by])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_account]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_category] FOREIGN KEY([category_id])
REFERENCES [dbo].[Category] ([category_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_image] FOREIGN KEY([image_id])
REFERENCES [dbo].[Image] ([image_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_image]
GO
USE [master]
GO
ALTER DATABASE [FStore] SET  READ_WRITE 
GO
USE [master]
GO
/****** Object:  Database [FStore]    Script Date: 2/21/2024 10:10:49 PM ******/
CREATE DATABASE [FStore]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FStore', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.DANGSINH\MSSQL\DATA\FStore.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FStore_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.DANGSINH\MSSQL\DATA\FStore_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [FStore] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [FStore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FStore] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FStore] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FStore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FStore] SET RECOVERY FULL 
GO
ALTER DATABASE [FStore] SET  MULTI_USER 
GO
ALTER DATABASE [FStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FStore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FStore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FStore] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [FStore] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [FStore] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'FStore', N'ON'
GO
ALTER DATABASE [FStore] SET QUERY_STORE = OFF
GO
USE [FStore]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[account_id] [int] IDENTITY(1,1) NOT NULL,
	[full_name] [varchar](100) NOT NULL,
	[mail] [varchar](100) NOT NULL,
	[address] [varchar](100) NOT NULL,
	[dob] [date] NULL,
	[gender] [bit] NULL,
	[phone] [varchar](10) NULL,
	[password] [varchar](100) NULL,
	[active] [bit] NOT NULL,
	[role_id] [int] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [varchar](100) NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[coupon_id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](100) NOT NULL,
	[discount_percent] [int] NOT NULL,
	[expiration_date] [date] NULL,
	[is_deleted] [bit] NULL,
 CONSTRAINT [PK__Coupons__58CF6389DA94C769] PRIMARY KEY CLUSTERED 
(
	[coupon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[image_link] [varchar](100) NULL,
	[create_date] [date] NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order_details]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_details](
	[order_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NOT NULL,
	[address] [varchar](100) NOT NULL,
	[create_date] [date] NULL,
	[shipping_date] [date] NULL,
	[required_date] [date] NULL,
	[status] [int] NULL,
	[payment_id] [int] NULL,
	[coupon_id] [int] NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[payment_name] [varchar](100) NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[product_id] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [varchar](100) NOT NULL,
	[price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[description] [varchar](max) NULL,
	[category_id] [int] NOT NULL,
	[image_id] [int] NOT NULL,
	[create_date] [date] NULL,
	[create_by] [int] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reviews]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reviews](
	[review_id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[customer_id] [int] NOT NULL,
	[rating] [int] NOT NULL,
	[comment] [varchar](max) NOT NULL,
	[create_date] [date] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[review_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2/21/2024 10:10:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](100) NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (4, N'Admin', N'sinhdqhe150849@fpt.edu.vn', N'Hung Yen', CAST(N'2001-03-14' AS Date), 1, N'0917076864', N'45310317335BD2615A915FC23D781BA7', 1, 1, 0)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (6, N'Staff 1', N'sinhdq@fpt.com', N'Ha Noi', CAST(N'2001-03-14' AS Date), 1, N'0917076864', N'1C694CFFD4999AFBA13E752D35DA0090', 1, 2, 0)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (7, N'Nguyen Van B', N'nguyenvanb@gmail.com', N'Ha Nam', CAST(N'2024-01-01' AS Date), 1, N'0985054398', N'62A2EFC2962A3BB876555A73212DA122', 1, 3, 0)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (8, N'Pham Van K', N'phamvank@gmail.com', N'Ha Noi', CAST(N'2024-01-04' AS Date), 1, N'0917076864', N'CF814721358D09942B255746542AD2A4', 1, 2, 1)
INSERT [dbo].[Account] ([account_id], [full_name], [mail], [address], [dob], [gender], [phone], [password], [active], [role_id], [is_deleted]) VALUES (1003, N'Dang Quoc Sinh', N'sinhphuung2001@gmail.com', N'Hung Yen', CAST(N'2001-03-14' AS Date), 1, N'0917076864', N'45310317335BD2615A915FC23D781BA7', 1, 3, 0)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (1, N'Samsung', 0)
INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (2, N'Apple', 0)
INSERT [dbo].[Category] ([category_id], [category_name], [is_deleted]) VALUES (3, N'Xiaomi', 0)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Coupons] ON 

INSERT [dbo].[Coupons] ([coupon_id], [code], [discount_percent], [expiration_date], [is_deleted]) VALUES (5, N'NORMAL', 0, NULL, 0)
SET IDENTITY_INSERT [dbo].[Coupons] OFF
GO
SET IDENTITY_INSERT [dbo].[Image] ON 

INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (1, N'samsungs24.jpg', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (2, N'samsungs22.jpg', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (3, N'samsungzfold5.jpg', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (4, N'Iphone15prm.png', CAST(N'2024-01-18' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (7, N'432532.png', CAST(N'2024-01-19' AS Date), 0)
INSERT [dbo].[Image] ([image_id], [image_link], [create_date], [is_deleted]) VALUES (8, N'Presentation1.png', CAST(N'2024-01-19' AS Date), 0)
SET IDENTITY_INSERT [dbo].[Image] OFF
GO
INSERT [dbo].[Order_details] ([order_id], [product_id], [quantity], [is_deleted]) VALUES (1, 2, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([order_id], [customer_id], [address], [create_date], [shipping_date], [required_date], [status], [payment_id], [coupon_id], [is_deleted]) VALUES (1, 1003, N'Hung Yen', CAST(N'2024-02-14' AS Date), NULL, NULL, 1, 2, 5, 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (1, N'PAYMENT ONLINE', 0)
INSERT [dbo].[Payment] ([payment_id], [payment_name], [is_deleted]) VALUES (2, N'COD', 0)
SET IDENTITY_INSERT [dbo].[Payment] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (2, N'Galaxy S24 Ultra', 33990000, 200, N'With the connectivity power of the Samsung Galaxy S24 Ultra, you can connect with the world easier than ever, overcoming all language barriers when communicating. Not only normal translation, S24 Ultra also has the ability to directly translate two-way calls in real time. You don''t even need to be connected to the Internet to understand what the other person is saying and vice versa. This feature makes it possible to communicate across borders. Whether it''s a voice or text message, S24 Ultra will quickly translate into Vietnamese and send back the message in the language of the other side.', 1, 1, CAST(N'2024-01-24' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (3, N'Samsung Galaxy S22 5G 128GB', 12490000, 200, N'Samsung Galaxy S22 is a leap forward in video technology in the mobile generation. At the same time, the phone also opens up a series of today''s leading breakthrough innovations, from the "flattering" flat beveled screen to the first advanced 4nm processor on the Samsung smartphone generation.', 1, 2, CAST(N'2024-01-30' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (4, N'Samsung Galaxy Z Fold5 5G', 31790000, 200, N'Samsung Galaxy Z Fold5 affirms its position as a pioneering folding phone, boldly breaking through old boundaries, leading with advanced Flex hinge technology. The device also opens up the most comprehensive range of smart experiences through a large screen, outstanding performance, S-Pen compatibility, allowing optimal multitasking, speeding up work effectively, giving users confidence. Connect flexibly with Galaxy Z Fold 5.', 1, 3, CAST(N'2024-01-18' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (5, N'iPhone 15 Pro Max 256GB', 32690000, 200, N'iPhone 15 Pro Max is the most advanced iPhone with the largest screen, best battery life, strongest configuration and super durable, super light aerospace-standard Titanium frame design. iPhone 15 Pro Max possesses Apple''s most outstanding features. Accordingly, users will experience a high-end iPhone with "huge" performance with A17 Pro chip, titanium frame, upgraded zoom capabilities, new action buttons,...', 2, 4, CAST(N'2024-01-30' AS Date), 4, 0)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (6, N'MacBook Air 13 inch M1 2020', 18000000, 200, N'The MacBook Air with the most groundbreaking performance ever is here. The all-new Apple M1 processor brings the power of the 2020 13-inch MacBook Air M1 beyond user expectations, able to run heavy tasks and amazing battery life.', 2, 7, CAST(N'2024-01-19' AS Date), 4, 1)
INSERT [dbo].[Product] ([product_id], [product_name], [price], [quantity], [description], [category_id], [image_id], [create_date], [create_by], [is_deleted]) VALUES (1002, N'ss15', 20000, 20000, N'ádfgnm', 1, 7, CAST(N'2024-01-23' AS Date), 4, 1)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (1, N'Admin', 0)
INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (2, N'Staff', 0)
INSERT [dbo].[Role] ([role_id], [role_name], [is_deleted]) VALUES (3, N'User', 0)
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [fk_account_role] FOREIGN KEY([role_id])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [fk_account_role]
GO
ALTER TABLE [dbo].[Order_details]  WITH CHECK ADD  CONSTRAINT [fk_item_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO
ALTER TABLE [dbo].[Order_details] CHECK CONSTRAINT [fk_item_product]
GO
ALTER TABLE [dbo].[Order_details]  WITH CHECK ADD  CONSTRAINT [fk_oder_items] FOREIGN KEY([order_id])
REFERENCES [dbo].[Orders] ([order_id])
GO
ALTER TABLE [dbo].[Order_details] CHECK CONSTRAINT [fk_oder_items]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_coupon] FOREIGN KEY([coupon_id])
REFERENCES [dbo].[Coupons] ([coupon_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_coupon]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_customer]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_payment] FOREIGN KEY([payment_id])
REFERENCES [dbo].[Payment] ([payment_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_payment]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_account] FOREIGN KEY([create_by])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_account]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_category] FOREIGN KEY([category_id])
REFERENCES [dbo].[Category] ([category_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [fk_product_image] FOREIGN KEY([image_id])
REFERENCES [dbo].[Image] ([image_id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [fk_product_image]
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [fk_review_customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Account] ([account_id])
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [fk_review_customer]
GO
ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [fk_review_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[Product] ([product_id])
GO
ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [fk_review_product]
GO
USE [master]
GO
ALTER DATABASE [FStore] SET  READ_WRITE 
GO

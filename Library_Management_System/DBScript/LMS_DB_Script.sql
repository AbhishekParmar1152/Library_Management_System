USE [master]
GO
/****** Object:  Database [Test2]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE DATABASE [Test2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Test2', FILENAME = N'C:\Users\amparmar\Test2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Test2_log', FILENAME = N'C:\Users\amparmar\Test2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Test2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Test2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Test2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Test2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Test2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Test2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Test2] SET ARITHABORT OFF 
GO
ALTER DATABASE [Test2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Test2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Test2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Test2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Test2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Test2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Test2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Test2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Test2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Test2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Test2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Test2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Test2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Test2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Test2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Test2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Test2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Test2] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Test2] SET  MULTI_USER 
GO
ALTER DATABASE [Test2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Test2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Test2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Test2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Test2] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Test2] SET QUERY_STORE = OFF
GO
USE [Test2]
GO
/****** Object:  UserDefinedTableType [dbo].[BookType]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE TYPE [dbo].[BookType] AS TABLE(
	[Book_Id] [int] NULL,
	[Book_Title] [nvarchar](50) NULL,
	[Book_Publication] [nvarchar](50) NULL,
	[Book_Author] [nvarchar](50) NULL,
	[Book_Category] [nvarchar](50) NULL,
	[No_Of_Copies_Actual] [int] NULL,
	[No_Of_Copies_Current] [int] NULL,
	[Book_Language] [nchar](10) NULL,
	[Book_Added_On] [date] NULL,
	[Library_Id] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[LibraryType]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE TYPE [dbo].[LibraryType] AS TABLE(
	[Library_Id] [int] NULL,
	[Library_Name] [nvarchar](50) NULL,
	[Library_Address] [nvarchar](100) NULL,
	[Library_City] [nvarchar](100) NULL,
	[Library_Pincode] [nvarchar](15) NULL,
	[Library_Create_Date] [date] NULL,
	[IsActive] [bit] NULL
)
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[Profile_Picture] [nvarchar](max) NULL,
	[Library_Id] [int] NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Book_Id] [int] IDENTITY(1,1) NOT NULL,
	[Book_Title] [nvarchar](50) NOT NULL,
	[Book_Publication] [nvarchar](50) NOT NULL,
	[Book_Author] [nvarchar](50) NOT NULL,
	[Book_Category] [nvarchar](50) NOT NULL,
	[No_Of_Copies_Actual] [int] NOT NULL,
	[No_Of_Copies_Current] [int] NOT NULL,
	[Book_Language] [nchar](10) NOT NULL,
	[Book_Added_On] [datetime] NOT NULL,
	[isDel] [nchar](10) NULL,
	[Library_Id] [int] NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Book_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IssueBook]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueBook](
	[Issue_Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Name] [nvarchar](50) NULL,
	[Book_Name] [nvarchar](50) NULL,
	[Issue_Date] [date] NULL,
	[Due_Date] [date] NULL,
	[Penalty] [int] NULL,
	[Returned] [nvarchar](1) NULL,
	[Return_Date] [date] NULL,
	[Library_Id] [int] NULL,
 CONSTRAINT [PK_IssueBook] PRIMARY KEY CLUSTERED 
(
	[Issue_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Library]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Library](
	[Library_Id] [int] IDENTITY(1,1) NOT NULL,
	[Library_Name] [nvarchar](50) NOT NULL,
	[Library_Address] [nvarchar](100) NOT NULL,
	[Library_City] [nvarchar](50) NULL,
	[Library_Pincode] [nvarchar](15) NULL,
	[Library_Create_Date] [datetime] NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Library] PRIMARY KEY CLUSTERED 
(
	[Library_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Penalty]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Penalty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Student_Name] [nvarchar](50) NULL,
	[Book_Name] [nvarchar](50) NULL,
	[Penalty] [nchar](10) NULL,
	[PaymentDate] [date] NULL,
 CONSTRAINT [PK_Penalty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20221201063355_Initial', N'3.0.0')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'0cbd0d44-14dd-4e53-8427-fbe0b1038ebd', N'User', N'USER', N'e643e4c9-d399-4b2f-8baf-fe646d570a84')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'2fd18cfb-5ec8-4bd0-b79c-fa3516c95f66', N'Librarian', N'LIBRARIAN', N'7d221248-2d1b-4a6a-aa57-77fc3a809fb7')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'87fbdd0b-0cbe-419e-800a-b65192eb6f4e', N'Admin', N'ADMIN', N'89bb3fda-a3b5-4e36-9011-2ab6d406b548')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'554629c1-2964-45a7-a75a-097a5b8088aa', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Librarian')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1002, N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2002, N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2003, N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2004, N'554629c1-2964-45a7-a75a-097a5b8088aa', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Librarian')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2005, N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (3002, N'c3e770f4-47c5-496b-9601-e411b806e9d4', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'User')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3d13f49b-7a78-4ae3-bb0a-b9c9a23a4847', N'0cbd0d44-14dd-4e53-8427-fbe0b1038ebd')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c3e770f4-47c5-496b-9601-e411b806e9d4', N'0cbd0d44-14dd-4e53-8427-fbe0b1038ebd')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e00b7680-edd4-4008-9d23-d44a160ada6b', N'0cbd0d44-14dd-4e53-8427-fbe0b1038ebd')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'eef082f2-11ae-4f2a-8e23-ac40f133ee6e', N'0cbd0d44-14dd-4e53-8427-fbe0b1038ebd')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'360bc84f-9ecb-4770-b196-a7960053f149', N'2fd18cfb-5ec8-4bd0-b79c-fa3516c95f66')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'554629c1-2964-45a7-a75a-097a5b8088aa', N'2fd18cfb-5ec8-4bd0-b79c-fa3516c95f66')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'87fbdd0b-0cbe-419e-800a-b65192eb6f4e')
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'360bc84f-9ecb-4770-b196-a7960053f149', N'Librarian2', N'LIBRARIAN2', N'abcd@gmail.com', N'ABCD@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEPg8xeBJyO+rpkm6ei/UPGeCWeueUI7jbYjIKPQz5Aaqato47uCWvOCofJ6vjf3x5g==', N'ZP2B3K7LTOIPUGSPBXQLHM3H2DP4SJLC', N'd178b26b-8e27-4740-a590-18ee47159720', N'9909988088', 0, 0, NULL, 1, 0, 0, NULL, 1)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'3d13f49b-7a78-4ae3-bb0a-b9c9a23a4847', N'User2', N'USER2', N'abcd@gmail.com', N'ABCD@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEHqM8EL0lJTQYmFgNNyJm8eHB16j9J8lbhTVuik53txVVTpjutt6Eg6ipadI5NBOkw==', N'5NSUXEZFSWVTCS3VKN7MPOAMFGDFIWJ3', N'e61820ba-ba0f-458e-8f15-8b567267fd5f', N'9987667587', 0, 0, NULL, 1, 0, 0, NULL, 1)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'554629c1-2964-45a7-a75a-097a5b8088aa', N'Librarian', N'LIBRARIAN', N'Librarian123@gmail.com', N'LIBRARIAN123@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAELQ5/riI+VCWgGgdTsaJKgqa4I1YrNn59vGZS9lT0ZFbTSd6ESzrH3ZdSOLqM57oIg==', N'2N3ET3LJEAFTAXDR2KU77NJFIGWF6LFO', N'06923f9e-1d19-40bd-b946-5f13324aa353', N'9987667587', 0, 0, CAST(N'2023-02-27T06:48:04.0321655+00:00' AS DateTimeOffset), 1, 0, 0, N'avatar-2.jpg', 4)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'7c4120dc-1689-41d4-883d-bf07c0a12830', N'Admin', N'ADMIN', N'Admin123@gmail.com', N'ADMIN123@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAELM8/iIPquPgfNrAfkWxKuNPkaVIKJFRiOYhis+5yRj4FGMpJhZkhEcPvz1Y+VFi3w==', N'65WHE77NRX27FCYRLPPYOR2WQ6FS7HLO', N'b9309ed2-140b-4aee-a118-e3db42eebbf0', N'9987667587', 0, 0, CAST(N'2023-02-17T09:58:57.6377799+00:00' AS DateTimeOffset), 1, 0, 0, N'user.png', NULL)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'c3e770f4-47c5-496b-9601-e411b806e9d4', N'User', N'USER', N'User123@gmail.com', N'USER123@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEKniPuzY5dFGH2guWMVq+vgVwREzDw+Lc7rfWBvh9InpYgrg9A32qb4s0/t3uCwKIA==', N'LPCJ3A5XDEFKXD2FN2BOKCAFELOG4NHT', N'1cc9c8b9-4155-4dab-af2f-31dbaa3b6392', N'9987667587', 0, 0, NULL, 1, 0, 0, N'avatar-1.jpg', 1)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'e00b7680-edd4-4008-9d23-d44a160ada6b', N'dhruvin', N'DHRUVIN', N'dhruvin.kakadiya@nexuslinkservices.in', N'DHRUVIN.KAKADIYA@NEXUSLINKSERVICES.IN', 0, N'AQAAAAEAACcQAAAAEI2Hun8e6wQTsbWhOu9GJiA9/5DPmLBPpIHBIY+o0Fy00PP650ZGhc5FWNjvlOCrAA==', N'PCWEFVWQGF7L3C6IS72X4QDGLP6GI3F7', N'bbde7c1a-16b9-4e46-a40f-addd92613c71', N'9987667587', 0, 0, NULL, 1, 0, NULL, NULL, 4)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [IsActive], [Profile_Picture], [Library_Id]) VALUES (N'eef082f2-11ae-4f2a-8e23-ac40f133ee6e', N'Abhishek', N'ABHISHEK', N'abhishek.parmar1152@gmail.com', N'ABHISHEK.PARMAR1152@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEBhmmoLBsidP2dgH7N6T61KaoVDAzFyRBO/yQAxcKMWhqpNrJqB9VZblKyXpo66YdQ==', N'MNKZGHZMEYRCOXKEOIHIQ3DHSC7GOETX', N'601ed1df-a51b-4cff-8630-be2c5810825e', N'9987667587', 0, 0, NULL, 1, 1, NULL, N'', 4)
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([Book_Id], [Book_Title], [Book_Publication], [Book_Author], [Book_Category], [No_Of_Copies_Actual], [No_Of_Copies_Current], [Book_Language], [Book_Added_On], [isDel], [Library_Id]) VALUES (2006, N'Artificial Intelligence', N'Technical Publications', N'Anamitra Deshmukh', N'Computer', 50, 50, N'English   ', CAST(N'2023-01-01T00:00:00.000' AS DateTime), NULL, 1)
INSERT [dbo].[Books] ([Book_Id], [Book_Title], [Book_Publication], [Book_Author], [Book_Category], [No_Of_Copies_Actual], [No_Of_Copies_Current], [Book_Language], [Book_Added_On], [isDel], [Library_Id]) VALUES (2007, N'Computer Networks', N'Pearson', N'Andrew S. Tanenbaum', N'Computer', 100, 100, N'English   ', CAST(N'2023-01-05T00:00:00.000' AS DateTime), NULL, 1)
INSERT [dbo].[Books] ([Book_Id], [Book_Title], [Book_Publication], [Book_Author], [Book_Category], [No_Of_Copies_Actual], [No_Of_Copies_Current], [Book_Language], [Book_Added_On], [isDel], [Library_Id]) VALUES (2008, N'Compiler Design', N'Atul Prakashan', N'Vinay Soni', N'IT', 300, 295, N'English   ', CAST(N'2023-01-09T00:00:00.000' AS DateTime), NULL, 3)
INSERT [dbo].[Books] ([Book_Id], [Book_Title], [Book_Publication], [Book_Author], [Book_Category], [No_Of_Copies_Actual], [No_Of_Copies_Current], [Book_Language], [Book_Added_On], [isDel], [Library_Id]) VALUES (2009, N'JAVA', N'Atul Prakashan', N'Sudha Rani S', N'Computer', 250, 240, N'English   ', CAST(N'2023-01-13T00:00:00.000' AS DateTime), NULL, 3)
INSERT [dbo].[Books] ([Book_Id], [Book_Title], [Book_Publication], [Book_Author], [Book_Category], [No_Of_Copies_Actual], [No_Of_Copies_Current], [Book_Language], [Book_Added_On], [isDel], [Library_Id]) VALUES (2010, N'Machine Learning', N'GK Publications', N'Sujit Bhattacharyya', N'Computer', 100, 100, N'English   ', CAST(N'2023-01-17T00:00:00.000' AS DateTime), NULL, 4)
INSERT [dbo].[Books] ([Book_Id], [Book_Title], [Book_Publication], [Book_Author], [Book_Category], [No_Of_Copies_Actual], [No_Of_Copies_Current], [Book_Language], [Book_Added_On], [isDel], [Library_Id]) VALUES (2011, N'MATHEMATICS-1', N'Mahajan Publishing House', N'A. R. Patel', N'Civil', 100, 100, N'English   ', CAST(N'2023-01-21T00:00:00.000' AS DateTime), NULL, 4)
SET IDENTITY_INSERT [dbo].[Books] OFF
SET IDENTITY_INSERT [dbo].[IssueBook] ON 

INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (1, N'User', N'Artificial Intelligence', CAST(N'2023-02-21' AS Date), CAST(N'2023-03-21' AS Date), 50, N'Y', CAST(N'2023-02-01' AS Date), 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (2, N'User', N'Computer Networks', CAST(N'2023-02-21' AS Date), CAST(N'2023-03-21' AS Date), 50, N'Y', CAST(N'2023-02-02' AS Date), 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (3, N'User', N'Compiler Design', CAST(N'2023-02-21' AS Date), CAST(N'2023-03-21' AS Date), 50, N'Y', CAST(N'2023-02-03' AS Date), 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (4, N'User', N'JAVA', CAST(N'2023-02-21' AS Date), CAST(N'2023-03-21' AS Date), 50, N'Y', CAST(N'2023-02-04' AS Date), 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (5, N'User', N'Machine Learning', CAST(N'2023-02-21' AS Date), CAST(N'2023-03-21' AS Date), 50, N'Y', CAST(N'2023-02-05' AS Date), 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (6, N'User', N'Artificial Intelligence', CAST(N'2023-01-21' AS Date), CAST(N'2023-02-21' AS Date), 50, N'N', NULL, 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (7, N'User', N'Computer Networks', CAST(N'2023-01-22' AS Date), CAST(N'2023-02-22' AS Date), 50, N'N', NULL, 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (8, N'User', N'Compiler Design', CAST(N'2023-01-23' AS Date), CAST(N'2023-02-23' AS Date), 50, N'N', NULL, 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (9, N'User', N'JAVA', CAST(N'2023-01-24' AS Date), CAST(N'2023-02-24' AS Date), 50, N'N', NULL, 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (10, N'User', N'Machine Learning', CAST(N'2023-01-25' AS Date), CAST(N'2023-02-25' AS Date), 50, N'N', NULL, 1)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (1006, N'Abhishek', N'Machine Learning', CAST(N'2023-02-24' AS Date), CAST(N'2023-03-24' AS Date), 50, N'Y', CAST(N'2024-05-21' AS Date), 4)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (2006, N'dhruvin', N'Machine Learning', CAST(N'2023-02-28' AS Date), CAST(N'2023-03-28' AS Date), 50, N'Y', CAST(N'2023-05-15' AS Date), 4)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (2007, N'dhruvin', N'MATHEMATICS-1', CAST(N'2023-02-28' AS Date), CAST(N'2023-03-28' AS Date), 50, N'Y', CAST(N'2023-02-28' AS Date), 4)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (3006, N'Abhishek', N'Machine Learning', CAST(N'2023-05-08' AS Date), CAST(N'2023-06-08' AS Date), 50, N'Y', CAST(N'2024-05-21' AS Date), 4)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (4006, N'dhruvin', N'Machine Learning', CAST(N'2023-05-15' AS Date), CAST(N'2023-06-15' AS Date), 50, N'Y', CAST(N'2023-05-15' AS Date), 4)
INSERT [dbo].[IssueBook] ([Issue_Id], [Student_Name], [Book_Name], [Issue_Date], [Due_Date], [Penalty], [Returned], [Return_Date], [Library_Id]) VALUES (5006, N'Abhishek', N'Machine Learning', CAST(N'2024-05-21' AS Date), CAST(N'2024-06-21' AS Date), NULL, N'Y', CAST(N'2024-05-21' AS Date), 4)
SET IDENTITY_INSERT [dbo].[IssueBook] OFF
SET IDENTITY_INSERT [dbo].[Library] ON 

INSERT [dbo].[Library] ([Library_Id], [Library_Name], [Library_Address], [Library_City], [Library_Pincode], [Library_Create_Date], [IsActive]) VALUES (1, N'MJ Library', N'2HFC+88P, Kavi Nanalal Marg, Ellisbridge', N'Ahmedabad', N'380016', CAST(N'2022-01-01T00:00:00.000' AS DateTime), 0)
INSERT [dbo].[Library] ([Library_Id], [Library_Name], [Library_Address], [Library_City], [Library_Pincode], [Library_Create_Date], [IsActive]) VALUES (3, N'VIVEKANAND READING LIBRARY', N'A-5, 3rd floor, Sardar mall, Nikol Gam Rd, Thakkar', N'Ahmedabad', N'382350', CAST(N'2022-01-03T00:00:00.000' AS DateTime), 0)
INSERT [dbo].[Library] ([Library_Id], [Library_Name], [Library_Address], [Library_City], [Library_Pincode], [Library_Create_Date], [IsActive]) VALUES (4, N'Vikram Sarabhai Library', N'Campus Entrance Road, I I M, Vastrapur', N'Ahmedabad', N'380015', CAST(N'2022-03-01T00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Library] OFF
SET IDENTITY_INSERT [dbo].[Penalty] ON 

INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (1, N'User', N'Artificial Intelligence', N'50        ', CAST(N'2023-02-01' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (2, N'User', N'Computer Networks', N'30        ', CAST(N'2023-02-02' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (3, N'User', N'Compiler Design', N'60        ', CAST(N'2023-02-03' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (4, N'User', N'JAVA', N'100       ', CAST(N'2023-02-04' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (5, N'User', N'Machine Learning', N'150       ', CAST(N'2023-02-05' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (1002, N'dhruvin', N'MATHEMATICS-1', N'50        ', CAST(N'2023-02-28' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (2002, N'Abhishek', N'Machine Learning', N'0         ', CAST(N'2023-05-08' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (3002, N'dhruvin', N'Machine Learning', N'0         ', CAST(N'2023-05-15' AS Date))
INSERT [dbo].[Penalty] ([Id], [Student_Name], [Book_Name], [Penalty], [PaymentDate]) VALUES (4002, N'Abhishek', N'Machine Learning', NULL, CAST(N'2024-05-21' AS Date))
SET IDENTITY_INSERT [dbo].[Penalty] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 5/27/2024 12:28:11 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
/****** Object:  StoredProcedure [dbo].[SP_Books]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Books]    
(    
@Book_Id INT = NULL,    
@Book_Title nvarchar(50)=NULL,
@Book_Publication nvarchar(50)=NULL,
@Book_Author nvarchar(50)=NULL,
@Book_Category NVARCHAR(50)=NULL,    
@No_Of_Copies_Actual int=NULL,    
@No_Of_Copies_Current int=NULL,      
@Book_Language nchar(10)=NULL,
@Book_Added_On Date=NULL,
@Library_Id int=NULL,
@status varchar(15)    
)  
AS
BEGIN
	if @status='Insert'      
begin    
insert into Books    
(Book_Title, Book_Publication, Book_Author, Book_Category, No_Of_Copies_Actual, No_Of_Copies_Current, Book_Language, Book_Added_On)    
values    
(@Book_Title,@Book_Publication,@Book_Author,@Book_Category,@No_Of_Copies_Actual,@No_Of_Copies_Current,@Book_Language,@Book_Added_On)    
End

IF (@status ='Display')      
BEGIN      
SELECT *      
FROM Books where Library_Id=@Library_Id 
END
IF (@status ='Update')  
BEGIN  
UPDATE Books
SET
Book_Title = @Book_Title
,Book_Publication = @Book_Publication
,Book_Author = @Book_Author
,Book_Category = @Book_Category
,No_Of_Copies_Actual = @No_Of_Copies_Actual
,No_Of_Copies_Current = @No_Of_Copies_Current
,Book_Language = @Book_Language
,Book_Added_On = @Book_Added_On
WHERE Books.Book_Id= @Book_Id
SELECT 'Update'  
END
IF (@status = 'Delete')      
BEGIN      
DELETE      
FROM Books      
WHERE Books.Book_Id = @Book_Id
SELECT 'Deleted'      
END
IF (@status = 'DeleteBook')      
BEGIN      
Update Books
SET
isDel = 'Y'     
WHERE Books.Book_Id= @Book_Id
END
IF (@status ='GetbyId')      
BEGIN      
SELECT *      
FROM Books where Books.Book_Id = @Book_Id
END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BulkUpload_Book]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[SP_BulkUpload_Book]
      @Books BookType READONLY
AS
BEGIN
      SET NOCOUNT ON;
 
      MERGE INTO Books E1
      USING @Books E2
      ON E1.Book_Id=E2.Book_Id
      WHEN MATCHED THEN
      UPDATE SET 
          E1.Book_Title = E2.Book_Title,
         E1.Book_Publication = E2.Book_Publication,
         E1.Book_Author = E2.Book_Author,
		 E1.Book_Category = E2.Book_Category,
         E1.No_Of_Copies_Actual = E2.No_Of_Copies_Actual,
         E1.No_Of_Copies_Current = E2.No_Of_Copies_Current,
		 E1.Book_Language = E2.Book_Language,
		 E1.Book_Added_On = E2.Book_Added_On,
		 E1.Library_Id = E2.Library_Id
      WHEN NOT MATCHED THEN
      INSERT(Book_Title,Book_Publication,Book_Author,Book_Category,No_Of_Copies_Actual,No_Of_Copies_Current,Book_Language,Book_Added_On,Library_Id) 
	  VALUES(E2.Book_Title, E2.Book_Publication, E2.Book_Author, E2.Book_Category, E2.No_Of_Copies_Actual, E2.No_Of_Copies_Current, E2.Book_Language, E2.Book_Added_On, E2.Library_Id);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_BulkUpload_Library]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE[dbo].[SP_BulkUpload_Library]
@Library LibraryType READONLY
AS
BEGIN
      SET NOCOUNT ON;

MERGE INTO Library E1
      USING @Library E2
      ON E1.Library_Id=E2.Library_Id
      WHEN MATCHED THEN
      UPDATE SET
         E1.Library_Name = E2.Library_Name,
         E1.Library_Address = E2.Library_Address,
		 E1.Library_City = E2.Library_City,
		 E1.Library_Pincode = E2.Library_Pincode,
         E1.Library_Create_Date = E2.Library_Create_Date,
		 E1.IsActive = E2.IsActive
      WHEN NOT MATCHED THEN
      INSERT(Library_Name, Library_Address, Library_City, Library_Pincode, Library_Create_Date, IsActive)

      VALUES(E2.Library_Name, E2.Library_Address, E2.Library_City, E2.Library_Pincode, E2.Library_Create_Date, E2.IsActive);
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Issue_Book]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Issue_Book]    
(    
@Student_Name nvarchar(50) = NULL,    
@Book_Name nvarchar(50)=NULL,
@Issue_Date date=NULL,
@Due_Date date=NULL,
@Penalty nchar(10)=NULL,
@Library_Id int=NULL,
@status varchar(15)    
)  
AS
BEGIN
	
IF (@status ='Display')      
BEGIN 
select * from IssueBook
END

IF (@status ='AddPenalty')      
BEGIN 
insert into Penalty(Student_Name,Book_Name,Penalty,PaymentDate) values(@Student_Name,@Book_Name, @Penalty,SYSDATETIME())
END

IF (@status ='GetBook')      
BEGIN 
select Book_Name from IssueBook
where Student_Name = @Student_Name and Returned='N';
END

IF (@status ='Insert')      
BEGIN 
insert into IssueBook(Student_Name,Book_Name,Library_Id,Issue_Date,Due_Date,Penalty,Returned) values(@Student_Name,@Book_Name,@Library_Id,SYSDATETIME(),dateadd(m, 1, getdate()),0,'N')
END

IF (@status ='BookHistory')      
BEGIN 
select * from IssueBook Where IssueBook.Student_Name=@Student_Name AND Returned='Y'
END

IF (@status ='PenaltyChart')      
BEGIN 
select Penalty,Due_Date from IssueBook where Student_Name=@Student_Name and Returned='N'
END

IF (@status ='PenaltyChartLibrarian')      
BEGIN 
select Book_Title,Book_Added_On from Books 
END

IF (@status ='Profile')      
BEGIN 
select UserName,Email,PhoneNumber from AspNetUsers Where AspNetUsers.UserName = @Student_Name
END

IF (@status ='Delete')      
BEGIN 
 Update IssueBook set Penalty=@Penalty,Returned='Y',Return_Date=SYSDATETIME() where Student_Name=@Student_Name and Book_Name=@Book_Name;
END
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Manage_Librarian]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Manage_Librarian]    
(    
@Id nvarchar(450) = NULL,    
@UserName nvarchar(256)=NULL,
@NormalizedUserName nvarchar(256)=NULL,
@Email nvarchar(256)=NULL,
@NormalizedEmail NVARCHAR(256)=NULL,    
@EmailConfirmed bit=NULL,    
@PasswordHash nvarchar(max)=NULL, 
@SecurityStamp nvarchar(max)=NULL, 
@ConcurrencyStamp nvarchar(max)=NULL, 
@PhoneNumber nvarchar(max)=NULL, 
@PhoneNumberConfirmed bit=NULL,
@TwoFactorEnabled bit=NULL,
@LockoutEnd datetimeoffset(7)=NULL,
@LockoutEnabled bit=NULL,
@AccessFailedCount int=NULL,
@Library_Id int=NULL,
@status varchar(15)    
)  
AS
BEGIN
	
IF (@status ='Display')      
BEGIN 
select ans.* from AspNetUsers ans
INNER JOIN AspNetUserRoles ON ans.Id = AspNetUserRoles.UserId and ans.IsActive=0 OR ans.IsActive=NULL 
INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'Librarian';
END
IF (@status = 'DeleteLibrarian')      
BEGIN      
UPDATE AspNetUsers      
SET AspNetUsers.IsActive = 1      
WHERE AspNetUsers.Id = @Id
SELECT 'Deleted'      
END
IF (@status ='UpdateLibrarian')  
BEGIN  
UPDATE AspNetUsers  
SET
UserName = @UserName
,Email = @Email
,PhoneNumber = @PhoneNumber
,Library_Id = @Library_Id
WHERE AspNetUsers.Id = @Id
SELECT 'Update'  
END
IF (@status ='GetbyId')      
BEGIN      
SELECT *      
FROM AspNetUsers where AspNetUsers.Id = @Id
END
END

  
GO
/****** Object:  StoredProcedure [dbo].[SP_Manage_User]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Manage_User]    
(    
@Id nvarchar(450) = NULL,    
@UserName nvarchar(256)=NULL,
@NormalizedUserName nvarchar(256)=NULL,
@Email nvarchar(256)=NULL,
@NormalizedEmail NVARCHAR(256)=NULL,    
@EmailConfirmed bit=NULL,    
@PasswordHash nvarchar(max)=NULL, 
@SecurityStamp nvarchar(max)=NULL, 
@ConcurrencyStamp nvarchar(max)=NULL, 
@PhoneNumber nvarchar(max)=NULL, 
@PhoneNumberConfirmed bit=NULL,
@TwoFactorEnabled bit=NULL,
@LockoutEnd datetimeoffset(7)=NULL,
@LockoutEnabled bit=NULL,
@AccessFailedCount int=NULL,
@Library_Id int=Null,
@status varchar(15)    
)  
AS
BEGIN
	
IF (@status ='Display')      
BEGIN 
select ans.* from AspNetUsers ans
INNER JOIN AspNetUserRoles ON ans.Id = AspNetUserRoles.UserId
INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetRoles.Name = 'User' AND ans.Library_Id=@Library_Id;
END
IF (@status = 'DeleteUser')      
BEGIN      
DELETE      
FROM AspNetUsers      
WHERE AspNetUsers.Id = @Id
DELETE
FROM AspNetUserRoles
WHERE AspNetUserRoles.UserId = @Id
SELECT 'Deleted'      
END
IF (@status ='UpdateUser')  
BEGIN  
UPDATE AspNetUsers  
SET
UserName = @UserName
,Email = @Email
,PhoneNumber = @PhoneNumber
WHERE AspNetUsers.Id = @Id
SELECT 'Update'  
END
IF (@status ='GetUserbyId')      
BEGIN      
SELECT *      
FROM AspNetUsers where AspNetUsers.Id = @Id
END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ManageLibraries]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure[dbo].[SP_ManageLibraries]    
(    
@Library_Id INT = NULL,    
@Library_Name nvarchar(50)=NULL,
@Library_Address nvarchar(50)=NULL,
@Library_City nvarchar(50)=NULL,
@Library_Pincode nvarchar(50)=NULL,
@Library_Create_Date DATE=NULL,
@status varchar(15)    
)      
as      
begin 
 
if @status='Insert'      
begin    
insert into Library    
(Library_Name, Library_Address, Library_Create_Date)    
values    
(@Library_Name, @Library_Address, @Library_Create_Date)    
End

IF (@status ='Display')      
BEGIN      
SELECT *      
FROM Library Where IsActive=0 OR IsActive=NULL;    
END
IF (@status ='UpdateLibrary')  
BEGIN  
UPDATE Library  
SET
Library_Name = @Library_Name
,Library_Address = @Library_Address
,Library_City = @Library_City
,Library_Pincode = @Library_Pincode
,Library_Create_Date = @Library_Create_Date
WHERE Library.Library_Id = @Library_Id
SELECT 'Update'  
END
IF (@status = 'Delete')      
BEGIN      
DELETE      
FROM Library      
WHERE Library.Library_Id = @Library_Id
SELECT 'Deleted'      
END
IF (@status = 'DeleteLibrary')      
BEGIN      
Update Library
SET
IsActive = 1     
WHERE Library.Library_Id= @Library_Id
END
IF (@status ='GetbyId')      
BEGIN      
SELECT *      
FROM Library where Library.Library_Id = @Library_Id
END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Profile]    Script Date: 5/27/2024 12:28:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Profile]    
(    
@User_Name nvarchar(50) = NULL,    
@Profile_Picture nvarchar(max)=NULL,
@status varchar(15)    
)  
AS
BEGIN
	
IF (@status ='Profile')      
BEGIN 
select UserName,Email,PhoneNumber,Profile_Picture from AspNetUsers Where AspNetUsers.UserName = @User_Name
END

IF (@status ='Upload_Image')      
BEGIN 
update AspNetUsers set Profile_Picture = @Profile_Picture where AspNetUsers.UserName = @User_Name 
END

END
GO
USE [master]
GO
ALTER DATABASE [Test2] SET  READ_WRITE 
GO

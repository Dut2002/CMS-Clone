USE [master]
GO
/****** Object:  Database [CMSDatabase]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE DATABASE [CMSDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CMSDatabase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\CMSDatabase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CMSDatabase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\CMSDatabase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CMSDatabase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CMSDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CMSDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CMSDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CMSDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CMSDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CMSDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [CMSDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CMSDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CMSDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CMSDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CMSDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CMSDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CMSDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CMSDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CMSDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CMSDatabase] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CMSDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CMSDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CMSDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CMSDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CMSDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CMSDatabase] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [CMSDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CMSDatabase] SET RECOVERY FULL 
GO
ALTER DATABASE [CMSDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [CMSDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CMSDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CMSDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CMSDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CMSDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CMSDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CMSDatabase', N'ON'
GO
ALTER DATABASE [CMSDatabase] SET QUERY_STORE = OFF
GO
USE [CMSDatabase]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/5/2024 9:10:59 PM ******/
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
/****** Object:  Table [dbo].[Assignments]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignments](
	[AssignmentId] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Deadline] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Assignments] PRIMARY KEY CLUSTERED 
(
	[AssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentCourses]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContentCourses](
	[ContentId] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ContentCourses] PRIMARY KEY CLUSTERED 
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseManagers]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseManagers](
	[CourseManagerId] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[IsStaff] [bit] NULL,
 CONSTRAINT [PK_CourseManagers] PRIMARY KEY CLUSTERED 
(
	[CourseManagerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses](
	[CourseId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](450) NOT NULL,
	[CourseName] [nvarchar](max) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[Enđate] [datetime2](7) NOT NULL,
	[Semester] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[CreatorId] [int] NOT NULL,
 CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents](
	[DocumentId] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[DocumentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DownloadFiles]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DownloadFiles](
	[DownloadId] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[LinkDownload] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DownloadFiles] PRIMARY KEY CLUSTERED 
(
	[DownloadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Links]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Links](
	[LinkId] [int] IDENTITY(1,1) NOT NULL,
	[ContentId] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[LinkAddress] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Links] PRIMARY KEY CLUSTERED 
(
	[LinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[Code] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/5/2024 9:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240321014128_Initial', N'6.0.27')
GO
SET IDENTITY_INSERT [dbo].[ContentCourses] ON 

INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (2, 1, N'1/8/2024 - 1/14/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (3, 1, N'1/15/2024 - 1/21/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (4, 1, N'1/22/2024 - 1/28/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (5, 1, N'1/29/2024 - 2/4/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (6, 1, N'2/5/2024 - 2/11/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (7, 1, N'2/12/2024 - 2/18/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (8, 1, N'2/19/2024 - 2/25/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (9, 1, N'2/26/2024 - 3/3/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (10, 1, N'3/4/2024 - 3/10/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (11, 1, N'3/11/2024 - 3/17/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (12, 1, N'3/18/2024 - 3/24/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (13, 1, N'3/25/2024 - 3/30/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (16, 6, N'5/1/2024 - 5/7/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (17, 6, N'5/8/2024 - 5/14/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (18, 6, N'5/15/2024 - 5/21/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (19, 6, N'5/22/2024 - 5/28/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (20, 6, N'5/29/2024 - 6/4/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (21, 6, N'6/5/2024 - 6/11/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (22, 6, N'6/12/2024 - 6/18/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (23, 6, N'6/19/2024 - 6/25/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (24, 6, N'6/26/2024 - 7/2/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (25, 6, N'7/3/2024 - 7/9/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (26, 6, N'7/10/2024 - 7/16/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (27, 6, N'7/17/2024 - 7/23/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (28, 6, N'7/24/2024 - 7/30/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (29, 6, N'7/31/2024 - 8/6/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (30, 6, N'8/7/2024 - 8/13/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (31, 6, N'8/14/2024 - 8/20/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (32, 6, N'8/21/2024 - 8/27/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (33, 6, N'8/28/2024 - 8/31/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (34, 7, N'1/30/2024 - 2/5/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (35, 7, N'2/6/2024 - 2/12/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (36, 7, N'2/13/2024 - 2/19/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (37, 7, N'2/20/2024 - 2/26/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (38, 7, N'2/27/2024 - 3/4/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (39, 7, N'3/5/2024 - 3/11/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (40, 7, N'3/12/2024 - 3/18/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (41, 7, N'3/19/2024 - 3/24/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (42, 8, N'5/1/2024 - 5/7/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (43, 8, N'5/8/2024 - 5/14/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (44, 8, N'5/15/2024 - 5/21/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (45, 8, N'5/22/2024 - 5/28/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (46, 8, N'5/29/2024 - 6/4/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (47, 8, N'6/5/2024 - 6/11/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (48, 8, N'6/12/2024 - 6/18/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (49, 8, N'6/19/2024 - 6/25/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (50, 8, N'6/26/2024 - 7/2/2024')
INSERT [dbo].[ContentCourses] ([ContentId], [CourseId], [Title]) VALUES (51, 8, N'7/3/2024 - 7/4/2024')
SET IDENTITY_INSERT [dbo].[ContentCourses] OFF
GO
SET IDENTITY_INSERT [dbo].[CourseManagers] ON 

INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (1, 1, 1, 1)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (6, 6, 1, 1)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (7, 7, 1, 1)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (9, 1, 2, 0)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (11, 6, 2, NULL)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (12, 7, 2, NULL)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (13, 1, 3, 0)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (14, 8, 3, 1)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (15, 8, 1, 1)
INSERT [dbo].[CourseManagers] ([CourseManagerId], [CourseId], [UserId], [IsStaff]) VALUES (16, 6, 3, NULL)
SET IDENTITY_INSERT [dbo].[CourseManagers] OFF
GO
SET IDENTITY_INSERT [dbo].[Courses] ON 

INSERT [dbo].[Courses] ([CourseId], [Code], [CourseName], [StartDate], [Enđate], [Semester], [Year], [CreatorId]) VALUES (1, N'EXE101', N'Khởi Nghiệp', CAST(N'2024-01-01T02:17:36.7780000' AS DateTime2), CAST(N'2024-03-30T02:17:36.7780000' AS DateTime2), 1, 2024, 1)
INSERT [dbo].[Courses] ([CourseId], [Code], [CourseName], [StartDate], [Enđate], [Semester], [Year], [CreatorId]) VALUES (6, N'PRN211', N'Kiến trúc và thiết kế phần mềm', CAST(N'2024-05-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-08-31T00:00:00.0000000' AS DateTime2), 2, 2024, 1)
INSERT [dbo].[Courses] ([CourseId], [Code], [CourseName], [StartDate], [Enđate], [Semester], [Year], [CreatorId]) VALUES (7, N'EXE101', N'Khởi Nghiệp ', CAST(N'2024-01-30T00:00:00.0000000' AS DateTime2), CAST(N'2024-03-24T00:00:00.0000000' AS DateTime2), 1, 2024, 1)
INSERT [dbo].[Courses] ([CourseId], [Code], [CourseName], [StartDate], [Enđate], [Semester], [Year], [CreatorId]) VALUES (8, N'MLN122', N'Thiết học Mác Lênin', CAST(N'2024-05-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-07-04T00:00:00.0000000' AS DateTime2), 2, 2024, 3)
SET IDENTITY_INSERT [dbo].[Courses] OFF
GO
SET IDENTITY_INSERT [dbo].[Documents] ON 

INSERT [dbo].[Documents] ([DocumentId], [ContentId], [Title], [Content]) VALUES (4, 34, N'Tuần Thứ 2', N'Nội dung học tuần 2')
SET IDENTITY_INSERT [dbo].[Documents] OFF
GO
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'EXE101', N'Khởi nghiệp 1')
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'EXE201', N'Khởi nghiệp 2')
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'MLN111', N'Trính trị Mác Lê-nin')
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'MLN122', N'Kinh tế chính trị Mác Lê-nin')
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'PRN211', N'.NET 1')
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'PRN221', N'.NET 2')
INSERT [dbo].[Subjects] ([Code], [Name]) VALUES (N'PRN231', N'.NET 3')
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Email], [Password], [FirstName], [LastName]) VALUES (1, N'duc@gmail.com', N'123', N'Duc', N'Cao')
INSERT [dbo].[Users] ([UserId], [Email], [Password], [FirstName], [LastName]) VALUES (2, N'user@example.com', N'string', N'123', N'123')
INSERT [dbo].[Users] ([UserId], [Email], [Password], [FirstName], [LastName]) VALUES (3, N'dut@gmail.com', N'123', N'dut', N'con')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Assignments_ContentId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Assignments_ContentId] ON [dbo].[Assignments]
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ContentCourses_CourseId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_ContentCourses_CourseId] ON [dbo].[ContentCourses]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CourseManagers_CourseId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_CourseManagers_CourseId] ON [dbo].[CourseManagers]
(
	[CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CourseManagers_UserId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_CourseManagers_UserId] ON [dbo].[CourseManagers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Courses_Code]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Courses_Code] ON [dbo].[Courses]
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Courses_CreatorId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Courses_CreatorId] ON [dbo].[Courses]
(
	[CreatorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Documents_ContentId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Documents_ContentId] ON [dbo].[Documents]
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DownloadFiles_ContentId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_DownloadFiles_ContentId] ON [dbo].[DownloadFiles]
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Links_ContentId]    Script Date: 4/5/2024 9:10:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Links_ContentId] ON [dbo].[Links]
(
	[ContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Assignments]  WITH CHECK ADD  CONSTRAINT [FK_Assignments_ContentCourses_ContentId] FOREIGN KEY([ContentId])
REFERENCES [dbo].[ContentCourses] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Assignments] CHECK CONSTRAINT [FK_Assignments_ContentCourses_ContentId]
GO
ALTER TABLE [dbo].[ContentCourses]  WITH CHECK ADD  CONSTRAINT [FK_ContentCourses_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([CourseId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContentCourses] CHECK CONSTRAINT [FK_ContentCourses_Courses_CourseId]
GO
ALTER TABLE [dbo].[CourseManagers]  WITH CHECK ADD  CONSTRAINT [FK_CourseManagers_Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([CourseId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseManagers] CHECK CONSTRAINT [FK_CourseManagers_Courses_CourseId]
GO
ALTER TABLE [dbo].[CourseManagers]  WITH CHECK ADD  CONSTRAINT [FK_CourseManagers_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[CourseManagers] CHECK CONSTRAINT [FK_CourseManagers_Users_UserId]
GO
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Subjects_Code] FOREIGN KEY([Code])
REFERENCES [dbo].[Subjects] ([Code])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Subjects_Code]
GO
ALTER TABLE [dbo].[Courses]  WITH CHECK ADD  CONSTRAINT [FK_Courses_Users_CreatorId] FOREIGN KEY([CreatorId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Courses] CHECK CONSTRAINT [FK_Courses_Users_CreatorId]
GO
ALTER TABLE [dbo].[Documents]  WITH CHECK ADD  CONSTRAINT [FK_Documents_ContentCourses_ContentId] FOREIGN KEY([ContentId])
REFERENCES [dbo].[ContentCourses] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Documents] CHECK CONSTRAINT [FK_Documents_ContentCourses_ContentId]
GO
ALTER TABLE [dbo].[DownloadFiles]  WITH CHECK ADD  CONSTRAINT [FK_DownloadFiles_ContentCourses_ContentId] FOREIGN KEY([ContentId])
REFERENCES [dbo].[ContentCourses] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DownloadFiles] CHECK CONSTRAINT [FK_DownloadFiles_ContentCourses_ContentId]
GO
ALTER TABLE [dbo].[Links]  WITH CHECK ADD  CONSTRAINT [FK_Links_ContentCourses_ContentId] FOREIGN KEY([ContentId])
REFERENCES [dbo].[ContentCourses] ([ContentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Links] CHECK CONSTRAINT [FK_Links_ContentCourses_ContentId]
GO
USE [master]
GO
ALTER DATABASE [CMSDatabase] SET  READ_WRITE 
GO

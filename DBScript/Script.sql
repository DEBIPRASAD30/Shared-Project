USE [master]
GO
/****** Object:  Database [ShopBridge]    Script Date: 06-06-2022 07:40:08 ******/
CREATE DATABASE [ShopBridge]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ShopBridge', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ShopBridge.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ShopBridge_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ShopBridge_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ShopBridge] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShopBridge].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShopBridge] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ShopBridge] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ShopBridge] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ShopBridge] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ShopBridge] SET ARITHABORT OFF 
GO
ALTER DATABASE [ShopBridge] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShopBridge] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShopBridge] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShopBridge] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShopBridge] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ShopBridge] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ShopBridge] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShopBridge] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ShopBridge] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShopBridge] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShopBridge] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShopBridge] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShopBridge] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShopBridge] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShopBridge] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShopBridge] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShopBridge] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShopBridge] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ShopBridge] SET  MULTI_USER 
GO
ALTER DATABASE [ShopBridge] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ShopBridge] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShopBridge] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShopBridge] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ShopBridge] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ShopBridge]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 06-06-2022 07:40:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NULL,
	[ProductCode] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [decimal](10, 2) NULL,
	[Image] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL CONSTRAINT [DF_Product_IsDeleted]  DEFAULT ((0)),
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[product_CRUD]    Script Date: 06-06-2022 07:40:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[product_CRUD] @id INT = NULL
	,@productName NVARCHAR(100) = NULL
	,@productCode NVARCHAR(50) = NULL
	,@description NVARCHAR(50) = NULL
	,@image NVARCHAR(50) = NULL
	,@price DECIMAL(10, 2) = 0
	,@createdBy INT = 0
	,@offsetvalue INT = 0
	,@pagingsize INT = 10
	,@operationtype NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@operationtype = 'SELECT')
	BEGIN
		SELECT ProductId
			,ProductCode
			,ProductName
			,[Description]
			,[Image] ImagePath
		FROM Product
		WHERE IsDeleted = 0
		ORDER BY ProductId 
		OFFSET @offsetvalue ROWS FETCH NEXT @pagingsize ROWS ONLY
	END

	IF (@operationtype = 'INSERT')
	BEGIN
		IF NOT EXISTS (
				SELECT 1
				FROM Product
				WHERE ProductCode = @productCode
				)
		BEGIN
			IF NOT EXISTS (
					SELECT 1
					FROM Product
					WHERE ProductName = @productName
					)
			BEGIN
				INSERT INTO Product (
					ProductCode
					,ProductName
					,Description
					,IMAGE
					,Price
					,IsDeleted
					,CreatedBy
					,CreatedDate
					)
				VALUES (
					@productCode
					,@productName
					,@description
					,@image
					,@price
					,0
					,@createdBy
					,GetDate()
					)

				SELECT 1;
			END
			ELSE
				SELECT 2;
		END
		ELSE
			SELECT 3;
	END

	IF (@operationtype = 'UPDATE')
	BEGIN
		IF NOT EXISTS (
				SELECT 1
				FROM Product
				WHERE ProductCode = @productCode
					AND ProductId != @id
				)
		BEGIN
			IF NOT EXISTS (
					SELECT 1
					FROM Product
					WHERE ProductName = @productName
						AND ProductId != @id
					)
			BEGIN
				UPDATE Product
				SET ProductCode = @productCode
					,ProductName = @productName
					,Description = @description
					,IMAGE = @image
					,Price = @price
					,ModifiedBy = @createdBy
					,ModifiedDate = GETDATE()
				WHERE ProductId = @id

				SELECT 1;
			END
			ELSE
				SELECT 2;
		END
		ELSE
			SELECT 3;
	END

	IF (@operationtype = 'SELECTONE')
	BEGIN
		SELECT ProductId
			,ProductCode
			,ProductName
			,[Description]
			,[Image] ImagePath
		FROM Product
		WHERE ProductId = @id
	END

	IF (@operationtype = 'DELETE')
	BEGIN
		UPDATE Product
		SET IsDeleted = 1
			,DeletedBy = 1
		WHERE ProductId = @id

		SELECT 1;
	END

	SET NOCOUNT OFF;
END
GO
USE [master]
GO
ALTER DATABASE [ShopBridge] SET  READ_WRITE 
GO

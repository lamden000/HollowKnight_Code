
USE Database1
GO
CREATE TABLE [dbo].[TaiKhoan] (
    [TenTaiKhoan] VARCHAR (50)    NOT NULL,
    [MatKhau]     VARCHAR (50)    NOT NULL,
    [Email]       VARCHAR (50)    NULL,
    [Tien]        DECIMAL (18, 0) DEFAULT ((200)) NULL,
    [Hoten]       NCHAR (50),
    [DN]    BIT DEFAULT (0) NOT NULL, 
    CONSTRAINT CK_TaiKhoan_Tien_NonNegative CHECK (Tien >= 0),
    PRIMARY KEY CLUSTERED ([TenTaiKhoan] ASC),
    UNIQUE NONCLUSTERED ([TenTaiKhoan] ASC)
);

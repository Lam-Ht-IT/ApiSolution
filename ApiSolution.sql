CREATE DATABASE QuanLyVanHoa
USE QuanLyVanHoa

--region Authorization Mangement System
--region Stored procedures of Users
CREATE TABLE Sys_User (
	UserID INT PRIMARY KEY IDENTITY(1,1),
	UserName NVARCHAR(50),
	Email NVARCHAR(50),
	Password NVARCHAR(100),
	Status BIT ,
	Note NVARCHAR(100)
);

GO
-- Get All Users
ALTER PROCEDURE UMS_GetListPaging
    @UserName NVARCHAR(50) = NULL, -- Updated to match the column size
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Calculate the total number of records matching the search criteria
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Sys_User
    WHERE @UserName IS NULL OR UserName LIKE '%' + @UserName + '%';

    -- Return data for the current page
    SELECT 
        UserID,
        UserName,
        Email,       -- Added Email field
        Password,
        Status,      -- Added Status field
        Note         -- Added Note field
    FROM Sys_User
    WHERE @UserName IS NULL OR UserName LIKE '%' + @UserName + '%'
    ORDER BY UserID  -- Can be adjusted based on sorting requirements
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END
GO
-- Get User by UserID
ALTER PROCEDURE UMS_GetByID
    @UserID int
AS
BEGIN
    SELECT * FROM Sys_User WHERE UserID = @UserID;
END
GO

-- Create User
ALTER PROCEDURE UMS_Create
    @UserName NVARCHAR(50),
    @Email NVARCHAR(50),
    @Password NVARCHAR(100),
    @Status BIT,
    @Note NVARCHAR(100)
AS
BEGIN
    -- Insert new user record
    INSERT INTO Sys_User (UserName, Email, Password, Status, Note)
    VALUES (@UserName, @Email, @Password, @Status, @Note);
END
GO
-- Update User
ALTER PROCEDURE UMS_Update
    @UserID INT,
    @UserName NVARCHAR(50),
    @Email NVARCHAR(50),
    @Password NVARCHAR(100),
    @Status BIT,
    @Note NVARCHAR(100)
AS
BEGIN
    -- Update existing user record
    UPDATE Sys_User
    SET 
        UserName = @UserName,
        Email = @Email,
        Password = @Password,
        Status = @Status,
        Note = @Note
    WHERE UserID = @UserID;
END
GO
-- Delete User
ALTER PROCEDURE UMS_Delete
    @UserID INT
AS
BEGIN
    -- Delete user record
    DELETE FROM Sys_User
    WHERE UserID = @UserID;
END
GO 
-- Veryfy Login
ALTER PROCEDURE VerifyLogin
    @UserName NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Sys_User
    WHERE UserName = @UserName AND Password = @Password;
END
GO
--endregion

--region Stored procedures of UserGroups
CREATE TABLE Sys_Group (
    GroupID INT PRIMARY KEY IDENTITY(1,1),
    GroupName NVARCHAR(50),
    Description NVARCHAR(100)
);
GO
-- Get All Group
ALTER PROCEDURE GMS_GetListPaging
    @GroupName NVARCHAR(50) = NULL, -- Updated to match the column size
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Calculate the total number of records matching the search criteria
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Sys_Group sug
    WHERE @GroupName IS NULL OR GroupName LIKE '%' + @GroupName + '%';

    -- Return data for the current page
    SELECT 
        sug.GroupID,
        sug.GroupName,
        sug.Description  
    FROM Sys_Group sug
    WHERE @GroupName IS NULL OR GroupName LIKE '%' + @GroupName + '%'
    ORDER BY sug.GroupID  -- Can be adjusted based on sorting requirements
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END
GO
-- Get Group By ID
CREATE PROC GMS_GetByID
	@GroupID INT 
AS 
BEGIN
	SELECT * FROM Sys_Group sg WHERE  sg.GroupID = @GroupID
END
GO

-- Create Group 
CREATE PROCEDURE GMS_Create
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Insert a new record into Sys_Group
    INSERT INTO Sys_Group (GroupName, Description)
    VALUES (@GroupName, @Description);
END
GO

-- Update Group
CREATE PROCEDURE GMS_Update
    @GroupID INT,
    @GroupName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Update the existing record in Sys_Group
    UPDATE Sys_Group
    SET GroupName = @GroupName,
        Description = @Description
    WHERE GroupID = @GroupID;
END
GO

-- Delete Group
CREATE PROCEDURE GMS_Delete
    @GroupID INT
AS
BEGIN
    -- Delete the record from Sys_Group
    DELETE FROM Sys_Group
    WHERE GroupID = @GroupID;
END
GO
--endregion

--region Stored procedures of Function
CREATE TABLE Sys_Function (
    FunctionID INT PRIMARY KEY IDENTITY(1,1),
    FunctionName NVARCHAR(50),
    Description NVARCHAR(100),
);
GO
-- GetListPaging of Function
CREATE PROCEDURE FMS_GetListPaging
    @FunctionName NVARCHAR(50) = NULL, -- Updated to match the column size
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Calculate the total number of records matching the search criteria
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM Sys_Function sf
    WHERE @FunctionName IS NULL OR FunctionName LIKE '%' + @FunctionName + '%';

    -- Return data for the current page
    SELECT 
        FunctionID,
        FunctionName,
        Description      
       
    FROM Sys_Function sf
    WHERE @FunctionName IS NULL OR FunctionName LIKE '%' + @FunctionName + '%'
    ORDER BY FunctionID  -- Can be adjusted based on sorting requirements
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Return the total number of records
    SELECT @TotalRecords AS TotalRecords;
END
GO

-- Get Function by ID
ALTER PROC FMS_GetByID
	@FunctionID int
AS
BEGIN
	SELECT * FROM Sys_Function sf WHERE sf.FunctionID = @FunctionID
END
GO

-- Create Function
CREATE PROCEDURE FMS_Create
    @FunctionName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Insert a new record into Sys_Function
    INSERT INTO Sys_Function (FunctionName, Description)
    VALUES (@FunctionName, @Description);
END
GO

-- Update Function
CREATE PROCEDURE FMS_Update
    @FunctionID INT,
    @FunctionName NVARCHAR(50),
    @Description NVARCHAR(100)
AS
BEGIN
    -- Update the existing record in Sys_Function
    UPDATE Sys_Function
    SET FunctionName = @FunctionName,
        Description = @Description
    WHERE FunctionID = @FunctionID;
END
GO

-- Delete Function
CREATE PROCEDURE FMS_Delete
    @FunctionID INT
AS
BEGIN
    -- Delete the record from Sys_Function
    DELETE FROM Sys_Function
    WHERE FunctionID = @FunctionID;
END
GO


--endregion

--region Stored procedures of UserInGroup
CREATE TABLE Sys_UserInGroup (
	UserInGroupID INT PRIMARY KEY IDENTITY(1,1) ,
    UserId INT NOT NULL,
    GroupId INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Sys_User(UserId),
    FOREIGN KEY (GroupId) REFERENCES Sys_Group(GroupId)
);
GO

CREATE PROCEDURE UIG_GetAll
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup;
END;
GO

CREATE PROCEDURE UIG_GetByGroupID
    @GroupID INT
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup
    WHERE GroupID = @GroupID;
END;
GO	

CREATE PROCEDURE UIG_GetByUserID
    @UserID INT
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup
    WHERE UserID = @UserID;
END;
GO	

CREATE PROCEDURE UIG_GetByID
    @UserInGroupID INT
AS
BEGIN
    SELECT UserInGroupID, UserID, GroupID
    FROM Sys_UserInGroup
    WHERE UserInGroupID = @UserInGroupID;
END;
GO

-- Add User to Group
CREATE PROC UIG_Create  
	@UserID INT ,
	@GroupID INT
AS
BEGIN
	INSERT INTO Sys_UserInGroup (UserId, GroupId)
	VALUES (@UserID,@GroupID);
END
GO

CREATE PROCEDURE UIG_Update
    @UserInGroupID INT,
    @UserID INT,
    @GroupID INT
AS
BEGIN
    UPDATE Sys_UserInGroup
    SET UserID = @UserID, GroupID = @GroupID
    WHERE UserInGroupID = @UserInGroupID;
END;
GO

ALTER PROCEDURE UIG_Delete
    @UserInGroupID INT
AS
BEGIN
    DELETE FROM Sys_UserInGroup
    WHERE UserInGroupID = @UserInGroupID;
END;
GO	
--endregion

--region Stored procedures of FunctionInGroup
CREATE TABLE Sys_FunctionInGroup (
	FunctionInGroupID INT PRIMARY KEY IDENTITY(1,1),
    FunctionId INT,
    GroupId INT,
	Permission INT NOT NULL, 
    FOREIGN KEY (FunctionId) REFERENCES Sys_Function(FunctionId),
    FOREIGN KEY (GroupId) REFERENCES Sys_Group(GroupId)
);
GO

CREATE PROCEDURE FIG_GetAll
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup;
END
GO

CREATE PROCEDURE FIG_GetByGroupID
    @GroupID INT
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup
    WHERE GroupID = @GroupID;
END
GO

CREATE PROCEDURE FIG_GetByFunctionID
    @FunctionID INT
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup
    WHERE FunctionID = @FunctionID;
END
GO

CREATE PROCEDURE FIG_GetByID
    @FunctionInGroupID INT
AS
BEGIN
    SELECT FunctionInGroupID, FunctionID, GroupID, Permission
    FROM Sys_FunctionInGroup
    WHERE FunctionInGroupID = @FunctionInGroupID;
END
GO

CREATE PROCEDURE FIG_Create
    @FunctionId INT,
    @GroupId INT,
    @Permission INT
AS
BEGIN
    INSERT INTO Sys_FunctionInGroup (FunctionId, GroupId, Permission)
    VALUES (@FunctionId, @GroupId, @Permission);
END;
GO	

CREATE PROCEDURE FIG_Update
    @FunctionId INT,
    @GroupId INT,
    @Permission INT
AS
BEGIN
    UPDATE Sys_FunctionInGroup
    SET Permission = @Permission
    WHERE FunctionId = @FunctionId AND GroupId = @GroupId;
END;
GO	

CREATE PROCEDURE FIG_Delete
    @FunctionInGroupID INT
AS
BEGIN
    DELETE FROM Sys_FunctionInGroup
    WHERE FunctionInGroupID = @FunctionInGroupID;
END
GO	

ALTER PROCEDURE FIG_GetUserPermissions
    @UserName NVARCHAR(50),
    @FunctionName NVARCHAR(50)
AS
BEGIN
    SELECT Permission
    FROM Sys_FunctionInGroup fg
    INNER JOIN Sys_Function f ON fg.FunctionID = f.FunctionID
    INNER JOIN Sys_UserInGroup ug ON fg.GroupID = ug.GroupID
    INNER JOIN Sys_User u ON ug.UserID = u.UserID
    WHERE u.UserName = @UserName AND f.FunctionName = @FunctionName
END
GO	
--endregion

--endregion

--region Category Mangement System
--region Stored procedures of DM_LoaiMauPieu
CREATE TABLE DM_LoaiMauPhieu
(
	LoaiMauPhieuID INT PRIMARY KEY IDENTITY(1,1),
	LoaiMauPhieuChaID INT DEFAULT 0,
	TenLoaiMauPhieu NVARCHAR(50),
	MaLoaiMauPhieu NVARCHAR(50),
	TrangThai BIT DEFAULT 0,
	GhiChu NVARCHAR(100),
	Loai INT DEFAULT 3
);
GO 


ALTER PROC LMP_GetAll
@TenLoaiMauPhieu NVARCHAR(50) = NULL,
@PageNumber INT = 1,
@PageSize INT = 20
AS
BEGIN
    -- Tính tổng số bản ghi phù hợp với điều kiện tìm kiếm
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM DM_LoaiMauPhieu
    WHERE @TenLoaiMauPhieu IS NULL OR TenLoaiMauPhieu LIKE '%' + @TenLoaiMauPhieu + '%';

    -- Trả về dữ liệu cho trang hiện tại
    SELECT 
        LoaiMauPhieuID,
		LoaiMauPhieuChaID,
        TenLoaiMauPhieu,
        MaLoaiMauPhieu,
		TrangThai,
		GhiChu,
		Loai
    FROM DM_LoaiMauPhieu
    WHERE @TenLoaiMauPhieu IS NULL OR TenLoaiMauPhieu LIKE '%' + @TenLoaiMauPhieu + '%'
    ORDER BY LoaiMauPhieuID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Trả về tổng số bản ghi
    SELECT @TotalRecords AS TotalRecords;
END
GO

CREATE PROC LMP_GetByID
	@LoaiMauPhieuID INT
AS	
BEGIN
	SELECT*FROM DM_LoaiMauPhieu WHERE LoaiMauPhieuID=@LoaiMauPhieuID;
END
GO

ALTER PROC LMP_Insert
	@TenLoaiMauPhieu NVARCHAR(50),
	@MaLoaiMauPhieu NVARCHAR(50),
	@GhiChu NVARCHAR(100)
AS
BEGIN
	INSERT INTO DM_LoaiMauPhieu (TenLoaiMauPhieu,MaLoaiMauPhieu,GhiChu) VALUES (@TenLoaiMauPhieu,@MaLoaiMauPhieu,@GhiChu);
END
GO

ALTER PROC LMP_Update
	@LoaiMauPhieuID INT,
	@TenLoaiMauPhieu NVARCHAR(50),
	@MaLoaiMauPhieu NVARCHAR(50),
	@GhiChu NVARCHAR(100)
AS
BEGIN
	UPDATE DM_LoaiMauPhieu
	SET TenLoaiMauPhieu = @TenLoaiMauPhieu, MaLoaiMauPhieu = @MaLoaiMauPhieu, GhiChu = @GhiChu WHERE LoaiMauPhieuID = @LoaiMauPhieuID;
END
GO	

CREATE PROC LMP_Delete
	@LoaiMauPhieuID INT
AS
BEGIN 
	DELETE FROM DM_LoaiMauPhieu WHERE LoaiMauPhieuID = @LoaiMauPhieuID;
END
GO
--endregion

--region Stored procedures of DonViTinh
CREATE TABLE DM_DonViTinh
(
	DonViTinhID INT PRIMARY KEY IDENTITY (1,1),
	TenDonViTinh NVARCHAR (50),
	MaDonViTinh NVARCHAR (50),
	TrangThai BIT,
	GhiChu NVARCHAR(100),
);
GO

ALTER PROC DVT_GetAll
@TenDonViTinh NVARCHAR(50) = NULL,
@PageNumber INT = 1,
@PageSize INT = 20
AS
BEGIN
    -- Tính tổng số bản ghi phù hợp với điều kiện tìm kiếm
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM DM_DonViTinh
    WHERE @TenDonViTinh IS NULL OR TenDonViTinh LIKE '%' + @TenDonViTinh + '%';

    -- Trả về dữ liệu cho trang hiện tại
    SELECT 
        DonViTinhID,
        TenDonViTinh,
        MaDonViTinh,
		TrangThai,
		GhiChu
    FROM DM_DonViTinh
    WHERE @TenDonViTinh IS NULL OR TenDonViTinh LIKE '%' + @TenDonViTinh + '%'
    ORDER BY DonViTinhID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Trả về tổng số bản ghi
    SELECT @TotalRecords AS TotalRecords;
END
GO

CREATE PROC DVT_GetByID
	@DonViTinhID INT
AS	
BEGIN
	SELECT*FROM DM_DonViTinh WHERE DonViTinhID=@DonViTinhID;
END
GO

CREATE PROC DVT_Insert
	@TenDonViTinh NVARCHAR(50),
	@MaDonViTinh NVARCHAR(50),
	@TrangThai BIT,
	@GhiChu NVARCHAR(100)
AS
BEGIN
	INSERT INTO DM_DonViTinh (TenDonViTinh,MaDonViTinh,TrangThai,GhiChu) VALUES (@TenDonViTinh,@MaDonViTinh,@TrangThai,@GhiChu);
END
GO

CREATE PROC DVT_Update
	@DonViTinhID INT,
	@TenDonViTinh NVARCHAR(50),
	@MaDonViTinh NVARCHAR(50),
	@TrangThai BIT,
	@GhiChu NVARCHAR(100)
AS
BEGIN
	UPDATE DM_DonViTinh
	SET TenDonViTinh = @TenDonViTinh, MaDonViTinh = @MaDonViTinh, TrangThai = @TrangThai, GhiChu = @GhiChu WHERE DonViTinhID = @DonViTinhID;
END
GO	

CREATE PROC DVT_Delete
	@DonViTinhID INT
AS
BEGIN 
	DELETE FROM DM_DonViTinh WHERE DonViTinhID = @DonViTinhID;
END
GO
--endregion

--region Stored procedures LoaiDiTich
CREATE TABLE DM_LoaiDiTich(
	LoaiDiTichID INT PRIMARY KEY IDENTITY(1,1),
	LoaiDiTichChaID INT DEFAULT 0,
	TenLoaiDiTich NVARCHAR(50),
	MaLoaiDiTich NVARCHAR(50) DEFAULT '',
	TrangThai BIT DEFAULT 0,
	GhiChu NVARCHAR(100),
	Loai INT DEFAULT 4
);
GO

ALTER PROC LDT_GetAll
@TenLoaiDiTich NVARCHAR(50) = NULL,
@PageNumber INT = 1,
@PageSize INT = 20
AS
BEGIN
    -- Tính tổng số bản ghi phù hợp với điều kiện tìm kiếm
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM DM_LoaiDiTich
    WHERE @TenLoaiDiTich IS NULL OR TenLoaiDiTich LIKE '%' + @TenLoaiDiTich + '%';

    -- Trả về dữ liệu cho trang hiện tại
    SELECT 
        LoaiDiTichID,
		LoaiDiTichChaID,
        TenLoaiDiTich,
        MaLoaiDiTich,
		TrangThai,
		GhiChu,
		Loai int
    FROM DM_LoaiDiTich
    WHERE @TenLoaiDiTich IS NULL OR TenLoaiDiTich LIKE '%' + @TenLoaiDiTich + '%'
    ORDER BY LoaiDiTichID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Trả về tổng số bản ghi
    SELECT @TotalRecords AS TotalRecords;
END
GO


CREATE PROC LDT_GetByID
	@LoaiDiTichID INT
AS	
BEGIN
	SELECT*FROM DM_LoaiDiTich WHERE LoaiDiTichID=@LoaiDiTichID;
END
GO

ALTER PROC LDT_Insert
	@TenLoaiDiTich NVARCHAR(50),
	@GhiChu NVARCHAR(100)
AS
BEGIN
	INSERT INTO DM_LoaiDiTich (TenLoaiDiTich,GhiChu) VALUES (@TenLoaiDiTich,@GhiChu);
END
GO

ALTER PROC LDT_Update
	@LoaiDiTichID INT,
	@TenLoaiDiTich NVARCHAR(50),
	@GhiChu NVARCHAR(100)
AS
BEGIN
	UPDATE DM_LoaiDiTich
	SET TenLoaiDiTich = @TenLoaiDiTich, GhiChu = @GhiChu WHERE LoaiDiTichID = @LoaiDiTichID;
END
GO	

ALTER PROC LDT_Delete
	@LoaiDiTichID INT
AS
BEGIN 
	DELETE FROM DM_LoaiDiTich WHERE LoaiDiTichID = @LoaiDiTichID;
END
GO
--endregion

--region Stored procedures of DiTichXepHang
CREATE TABLE DM_DiTichXepHang (
	DiTichXepHangID INT IDENTITY(1,1),
	DiTichXepHangChaID INT DEFAULT 0,
	TenDiTich NVARCHAR(50),
	MaDiTich NVARCHAR(50)  DEFAULT '',
	TrangThai BIT  DEFAULT 0,
	GhiChu NVARCHAR(100),
	Loai INT DEFAULT 0
);
GO

ALTER PROC DTXH_GetAll
    @TenDiTich NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Tính tổng số bản ghi phù hợp với điều kiện tìm kiếm
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM DM_DITICHXEPHANG
    WHERE @TenDiTich IS NULL OR TenDiTich LIKE '%' + @TenDiTich + '%';

    -- Trả về dữ liệu cho trang hiện tại
    SELECT 
        DiTichXepHangID,
		DiTichXepHangChaID,
        TenDiTich,
        MaDiTich,
        TrangThai,
        GhiChu,
        Loai
    FROM DM_DITICHXEPHANG
    WHERE @TenDiTich IS NULL OR TenDiTich LIKE '%' + @TenDiTich + '%'
    ORDER BY GhiChu
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Trả về tổng số bản ghi
    SELECT @TotalRecords AS TotalRecords;
END
GO

ALTER PROC DTXH_GetByID
    @DiTichXepHangID INT
AS	
BEGIN
    SELECT 
        DiTichXepHangID,
		DiTichXepHangChaID,
        TenDiTich,
        MaDiTich,
        TrangThai,
        GhiChu,
        Loai
    FROM DM_DITICHXEPHANG
    WHERE DiTichXepHangID = @DiTichXepHangID;
END
GO	

ALTER PROC DTXH_Insert
    @TenDiTich NVARCHAR(50),
    @GhiChu NVARCHAR(100)
AS
BEGIN
    INSERT INTO DM_DITICHXEPHANG (TenDiTich, GhiChu) 
    VALUES (@TenDiTich, @GhiChu);
END
GO

ALTER PROC DTXH_Update
    @DiTichXepHangID INT,
    @TenDiTich NVARCHAR(50),
    @GhiChu NVARCHAR(100)
AS
BEGIN
    UPDATE DM_DITICHXEPHANG
    SET 
        TenDiTich = @TenDiTich,
        GhiChu = @GhiChu
    WHERE DiTichXepHangID = @DiTichXepHangID;
END
GO

ALTER PROC DTXH_Delete
    @DiTichXepHangID INT
AS
BEGIN 
    DELETE FROM DM_DITICHXEPHANG 
    WHERE DiTichXepHangID = @DiTichXepHangID;
END
GO
--endregion

--region Stored procedures of DM_TieuChi
CREATE TABLE DM_TieuChi (
	TieuChiID INT PRIMARY KEY IDENTITY(1,1),
	MaTieuChi NVARCHAR(50),
	TenTieuChi NVARCHAR(50) NOT NULL,
	TieuChiChaID INT NULL,
	CONSTRAINT FK_TieuChi_TieuChiCha FOREIGN KEY (TieuChiChaID) REFERENCES DM_TieuChi(TieuChiID),
	GhiChu NVARCHAR(100),
	KieuDuLieuCot INT,
	TrangThai BIT,
	LoaiTieuChi INT,
);
GO

ALTER PROCEDURE TC_GetAll
    @TenTieuChi NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM DM_TieuChi
    WHERE @TenTieuChi IS NULL OR TenTieuChi LIKE '%' + @TenTieuChi + '%';

    SELECT 
        TieuChiID,
        MaTieuChi,
        TenTieuChi,
        TieuChiChaID,
        GhiChu,
        KieuDuLieuCot,
        TrangThai,
        LoaiTieuChi
    FROM 
        DM_TieuChi
    WHERE 
        @TenTieuChi IS NULL OR TenTieuChi LIKE '%' + @TenTieuChi + '%'
    ORDER BY 
        TieuChiID
    OFFSET (@PageNumber - 1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT @TotalRecords AS TotalRecords;
END
GO	

CREATE PROC TC_GetByID
	@TieuChiID INT
AS
BEGIN
	SELECT *FROM DM_TieuChi dtc WHERE dtc.TieuChiID = @TieuChiID;
END
GO

ALTER PROC TC_GetByName
	@TenTieuChi NVARCHAR(50)
AS
BEGIN
	SELECT *FROM DM_TieuChi dtc WHERE dtc.TenTieuChi LIKE '%' + @TenTieuChi + '%';
END	
GO	

ALTER PROC TC_Insert
	@MaTieuChi NVARCHAR(50),
    @TenTieuChi NVARCHAR(50),
    @TieuChiChaID INT = NULL,
    @GhiChu NVARCHAR(100) = NULL,
    @KieuDuLieuCot INT = NULL,
    @TrangThai BIT = NULL,
    @LoaiTieuChi INT = NULL
AS
BEGIN
    INSERT INTO DM_TieuChi (MaTieuChi, TenTieuChi, TieuChiChaID, GhiChu, KieuDuLieuCot, TrangThai, LoaiTieuChi)
    VALUES (@MaTieuChi, @TenTieuChi, @TieuChiChaID, @GhiChu, @KieuDuLieuCot, @TrangThai, @LoaiTieuChi);
END;  
GO
	
ALTER PROCEDURE TC_Update
    @TieuChiID INT,
    @MaTieuChi NVARCHAR(50),
    @TenTieuChi NVARCHAR(50),
    @TieuChiChaID INT = NULL,
    @GhiChu NVARCHAR(100) = NULL,
    @KieuDuLieuCot INT = NULL,
    @TrangThai BIT = NULL,
    @LoaiTieuChi INT = NULL
AS
BEGIN
    UPDATE DM_TieuChi
    SET 
        MaTieuChi = @MaTieuChi,
        TenTieuChi = @TenTieuChi,
        TieuChiChaID = @TieuChiChaID,
        GhiChu = @GhiChu,
        KieuDuLieuCot = @KieuDuLieuCot,
        TrangThai = @TrangThai,
        LoaiTieuChi = @LoaiTieuChi
    WHERE TieuChiID = @TieuChiID;
END;
GO

CREATE PROCEDURE TC_Delete
    @TieuChiID INT
AS
BEGIN
    DELETE FROM DM_TieuChi
    WHERE TieuChiID = @TieuChiID;
END;
GO
--endregion

--region Stored procedures of ChiTieu
CREATE TABLE DM_ChiTieu (
    ChiTieuID INT PRIMARY KEY IDENTITY(1,1),
    MaChiTieu NVARCHAR(50) NOT NULL,        
    TenChiTieu NVARCHAR(50) NOT NULL,      
    ChiTieuChaID INT NULL,                  
    GhiChu NVARCHAR(100) NULL,               
    TrangThai BIT NOT NULL,                 
    LoaiMauPhieuID INT NOT NULL,                 
    FOREIGN KEY (ChiTieuChaID) REFERENCES DM_ChiTieu(ChiTieuID) 
);  
GO

ALTER PROCEDURE [dbo].[CT_GetAll]
    @TenChiTieu NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    -- Kiểm tra biến đầu vào
    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 SET @PageSize = 20;

    -- CTE để xử lý phân cấp và tìm kiếm
    WITH RecursiveCTE AS (
        -- Anchor member: Bắt đầu với các node gốc (nơi ChiTieuChaID là NULL)
        SELECT 
            ChiTieuID,
            MaChiTieu,
            TenChiTieu,
            ChiTieuChaID,
            GhiChu,
            TrangThai,
            LoaiMauPhieuID,
            0 AS Level,
            CAST('/' + CONVERT(NVARCHAR(50), ChiTieuID) AS NVARCHAR(50)) AS Hierarchy
        FROM 
            DM_ChiTieu
        WHERE 
            ChiTieuChaID IS NULL

        UNION ALL

        -- Recursive member: Join the table with itself to find children
        SELECT 
            c.ChiTieuID,
            c.MaChiTieu,
            c.TenChiTieu,
            c.ChiTieuChaID,
            c.GhiChu,
            c.TrangThai,
            c.LoaiMauPhieuID,
            cte.Level + 1 AS Level,
            CAST(cte.Hierarchy + '/' + CONVERT(NVARCHAR(50), c.ChiTieuID) AS NVARCHAR(50)) AS Hierarchy
        FROM 
            DM_ChiTieu c
        INNER JOIN 
            RecursiveCTE cte ON c.ChiTieuChaID = cte.ChiTieuID
    ),
    -- CTE phụ để lọc các phần tử khớp với từ khóa tìm kiếm
    FilteredCTE AS (
        SELECT 
            ChiTieuID,
            MaChiTieu,
            TenChiTieu,
            ChiTieuChaID,
            GhiChu,
            TrangThai,
            LoaiMauPhieuID,
            Level,
            Hierarchy
        FROM 
            RecursiveCTE
        WHERE 
            @TenChiTieu IS NULL
            OR LOWER(TenChiTieu) LIKE '%' + LOWER(@TenChiTieu) + '%'
    ),
    -- CTE phụ để xác định các phần tử cha và phần tử con của các phần tử cần thiết
    AllParentsCTE AS (
        SELECT 
            c.*
        FROM 
            FilteredCTE fc
        INNER JOIN 
            RecursiveCTE c ON c.ChiTieuID = fc.ChiTieuChaID
        UNION ALL
        SELECT 
            c.*
        FROM 
            AllParentsCTE p
        INNER JOIN 
            RecursiveCTE c ON c.ChiTieuID = p.ChiTieuChaID
    ),
    AllChildrenCTE AS (
        SELECT 
            c.*
        FROM 
            FilteredCTE fc
        INNER JOIN 
            RecursiveCTE c ON c.ChiTieuChaID = fc.ChiTieuID
        UNION ALL
        SELECT 
            c.*
        FROM 
            AllChildrenCTE p
        INNER JOIN 
            RecursiveCTE c ON c.ChiTieuChaID = p.ChiTieuID
    ),
    -- Kết hợp tất cả các phần tử cần thiết và loại bỏ các bản ghi trùng lặp
    CombinedCTE AS (
        SELECT DISTINCT * FROM FilteredCTE
        UNION
        SELECT DISTINCT * FROM AllParentsCTE
        UNION
        SELECT DISTINCT * FROM AllChildrenCTE
    )
    -- Truy vấn phân cấp với phân trang và tính toán tổng số bản ghi
    SELECT 
        ChiTieuID,
        MaChiTieu,
        TenChiTieu,
        ChiTieuChaID,
        GhiChu,
        TrangThai,
        LoaiMauPhieuID,
        Level,
        Hierarchy,
        TotalRecords = (SELECT COUNT(*) FROM CombinedCTE)
    FROM 
        CombinedCTE
    ORDER BY 
        Hierarchy
    OFFSET (@PageNumber - 1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY
    OPTION (MAXRECURSION 0);
END;
GO

ALTER PROCEDURE CT_Insert
    @MaChiTieu NVARCHAR(50),
    @TenChiTieu NVARCHAR(50),
    @ChiTieuChaID INT = NULL,
    @GhiChu NVARCHAR(100) = NULL,
    @TrangThai BIT,
    @LoaiMauPhieuID INT
AS
BEGIN
    INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
    VALUES (@MaChiTieu, @TenChiTieu, @ChiTieuChaID, @GhiChu, @TrangThai, @LoaiMauPhieuID);
    
    SELECT SCOPE_IDENTITY() AS NewChiTieuID;  -- Return the newly created ID
END;
GO

ALTER PROCEDURE CT_GetByID
    @ChiTieuID INT
AS
BEGIN
    SELECT * FROM DM_ChiTieu WHERE ChiTieuID = @ChiTieuID;
END; 
GO

ALTER PROCEDURE CT_Update
    @ChiTieuID INT,
    @MaChiTieu NVARCHAR(50),
    @TenChiTieu NVARCHAR(50),
    @ChiTieuChaID INT = NULL,
    @GhiChu NVARCHAR(100) = NULL,
    @TrangThai BIT,
    @LoaiMauPhieuID INT
AS
BEGIN
    UPDATE DM_ChiTieu
    SET MaChiTieu = @MaChiTieu,
        TenChiTieu = @TenChiTieu,
        ChiTieuChaID = @ChiTieuChaID,
        GhiChu = @GhiChu,
        TrangThai = @TrangThai,
        LoaiMauPhieuID = @LoaiMauPhieuID
    WHERE ChiTieuID = @ChiTieuID;
END;
GO

ALTER PROCEDURE CT_Delete
    @ChiTieuID INT
AS
BEGIN
    DELETE FROM DM_ChiTieu WHERE ChiTieuID = @ChiTieuID;
END;
GO
--endregion

--region Stored procedures of DM_KyBaoCao
CREATE TABLE DM_KyBaoCao (
	KyBaoCaoID INT IDENTITY(1,1) PRIMARY KEY,
	TenKyBaoCao NVARCHAR(50) NOT NULL,
    TrangThai BIT NOT NULL,
    GhiChu NVARCHAR(100),
    LoaiKyBaoCao INT
);   ALTER TABLE DM_KyBaoCao ALTER COLUMN GhiChu NVARCHAR(100)
GO 

ALTER PROCEDURE KBC_GetAll
    @TenKyBaoCao NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    -- Tính tổng số bản ghi phù hợp với điều kiện tìm kiếm (không phân biệt chữ hoa, chữ thường)
    DECLARE @TotalRecords INT;
    SELECT @TotalRecords = COUNT(*)
    FROM DM_KyBaoCao dkbc
    WHERE @TenKyBaoCao IS NULL OR LOWER(TenKyBaoCao) LIKE '%' + LOWER(@TenKyBaoCao) + '%';

    -- Trả về dữ liệu cho trang hiện tại
    SELECT 
        KyBaoCaoID,
        TenKyBaoCao,
        TrangThai,
        GhiChu,
        LoaiKyBaoCao
    FROM DM_KyBaoCao
    WHERE @TenKyBaoCao IS NULL OR LOWER(TenKyBaoCao) LIKE '%' + LOWER(@TenKyBaoCao) + '%'
    ORDER BY KyBaoCaoID
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Trả về tổng số bản ghi
    SELECT @TotalRecords AS TotalRecords;
END;
GO	

CREATE PROC KBC_GetByID
	@KyBaoCaoID INT
AS
BEGIN
	SELECT * FROM DM_KyBaoCao dkbc  WHERE dkbc.KyBaoCaoID = @KyBaoCaoID 
END
GO

ALTER PROCEDURE KBC_Insert
    @TenKyBaoCao NVARCHAR(50),
    @TrangThai BIT,
    @GhiChu NVARCHAR(100) = NULL,
    @LoaiKyBaoCao INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO DM_KyBaoCao (TenKyBaoCao, TrangThai, GhiChu, LoaiKyBaoCao)
    VALUES (@TenKyBaoCao, @TrangThai, @GhiChu, @LoaiKyBaoCao);

    -- Trả về ID của bản ghi vừa tạo
    SELECT SCOPE_IDENTITY() AS KyBaoCaoID;
END;
GO	

ALTER PROCEDURE KBC_Update
    @KyBaoCaoID INT,
    @TenKyBaoCao NVARCHAR(50),
    @TrangThai BIT,
    @GhiChu NVARCHAR(100) = NULL,
    @LoaiKyBaoCao INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DM_KyBaoCao
    SET 
        TenKyBaoCao = @TenKyBaoCao,
        TrangThai = @TrangThai,
        GhiChu = @GhiChu,
        LoaiKyBaoCao = @LoaiKyBaoCao
    WHERE 
        KyBaoCaoID = @KyBaoCaoID;

    -- Trả về số bản ghi đã được cập nhật
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

CREATE PROCEDURE KBC_Delete
    @KyBaoCaoID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM DM_KyBaoCao
    WHERE KyBaoCaoID = @KyBaoCaoID;

    -- Trả về số bản ghi đã bị xóa
    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO	

--endregion
--endregion

--region Insert Table
--region Insert records into Categories
-- Insert records into Sys_User table, including an admin user
 
INSERT INTO DM_DITICHXEPHANG ( TenDiTich,GhiChu ) VALUES
( N'Di Tích Quốc Gia','3' ),
( N'Di Tích Quốc Gia Đặc Biệt','2' ),
( N'Di Tích Cấp Tỉnh','1' )
GO

-- Insert first record
INSERT INTO DM_DonViTinh (TenDonViTinh, MaDonViTinh, TrangThai, GhiChu)
VALUES (N'Kilogram', N'KG', 1, N'Đơn vị đo khối lượng'),
(N'Hộp', N'HOP', 1, N'Đơn vị đo đếm đóng gói'),
(N'Mét', N'M', 1, N'Đơn vị đo chiều dài'),
(N'Lit', N'L', 1, N'Đơn vị đo thể tích'),
(N'Cái', N'CAI', 1, N'Đơn vị đếm số lượng');
GO

INSERT INTO DM_TieuChi (MaTieuChi, TenTieuChi, TieuChiChaID, GhiChu, KieuDuLieuCot, TrangThai, LoaiTieuChi)
VALUES 
('TC001', 'Tiêu chí 1', NULL, 'Ghi chú 1', 1, 1, 1),
('TC002', 'Tiêu chí 2', NULL, 'Ghi chú 2', 2, 1, 1),
('TC003', 'Tiêu chí 3', 1, 'Ghi chú 3', 1, 0, 2),
('TC004', 'Tiêu chí 4', 1, 'Ghi chú 4', 2, 1, 1),
('TC005', 'Tiêu chí 5', 2, 'Ghi chú 5', 3, 0, 2),
('TC006', 'Tiêu chí 6', 2, 'Ghi chú 6', 1, 1, 1),
('TC007', 'Tiêu chí 7', 3, 'Ghi chú 7', 2, 1, 2),
('TC008', 'Tiêu chí 8', 4, 'Ghi chú 8', 3, 0, 1),
('TC009', 'Tiêu chí 9', 5, 'Ghi chú 9', 1, 1, 1),
('TC010', 'Tiêu chí 10', 6, 'Ghi chú 10', 2, 1, 2);
GO

INSERT INTO DM_LoaiMauPhieu (LoaiMauPhieuChaID, TenLoaiMauPhieu, MaLoaiMauPhieu, GhiChu)
VALUES
(0, N'Phiếu thu', N'PT', N'Mẫu phiếu thu tiền'),
(0, N'Phiếu chi', N'PC', N'Mẫu phiếu chi tiền'),
(0, N'Phiếu nhập kho', N'PNK', N'Mẫu phiếu nhập kho hàng hóa'),
(0, N'Phiếu xuất kho', N'PXK', N'Mẫu phiếu xuất kho hàng hóa'),
(0, N'Phiếu bảo hành', N'PBH', N'Mẫu phiếu bảo hành sản phẩm');
GO

--region Root Level ChiTieus
INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT001', N'Kế hoạch tài chính', NULL, N'Chi tiêu tài chính', 1, 1);

INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT002', N'Quản lý dự án', NULL, N'Chi tiêu dự án', 1, 2);

-- Children of ChiTieu 'CT001'
INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT001-01', N'Ngân sách hàng tháng', 1, N'Ngân sách chi tiêu hàng tháng', 1, 1);

INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT001-02', N'Ngân sách hàng năm', 1, N'Ngân sách chi tiêu hàng năm', 1, 1);

-- Children of ChiTieu 'CT002'
INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT002-01', N'Kế hoạch dự án A', 2, N'Kế hoạch chi tiêu dự án A', 1, 2);

INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT002-02', N'Kế hoạch dự án B', 2, N'Kế hoạch chi tiêu dự án B', 1, 2);

-- Children of 'CT001-01'
INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT001-01-01', N'Ngân sách ăn uống', 3, N'Chi tiêu cho ăn uống', 1, 1);

INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT001-01-02', N'Ngân sách nhà ở', 3, N'Chi tiêu cho nhà ở', 1, 1);

-- Children of 'CT002-01'
INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT002-01-01', N'Ngân sách nhân sự', 5, N'Chi tiêu cho nhân sự dự án A', 1, 2);

INSERT INTO DM_ChiTieu (MaChiTieu, TenChiTieu, ChiTieuChaID, GhiChu, TrangThai, LoaiMauPhieuID)
VALUES 
('CT002-01-02', N'Ngân sách thiết bị', 5, N'Chi tiêu cho thiết bị dự án A', 1, 2);
GO
--endregion


INSERT INTO DM_LoaiDiTich (TenLoaiDiTich, GhiChu)
VALUES (N'Loại di tích văn hóa', N'Di tích văn hóa nổi bật'),
(N'Loại di tích lịch sử', N'Di tích lịch sử quan trọng'),
(N'Loại di tích kiến trúc', N'Di tích kiến trúc cổ'),
(N'Loại di tích khảo cổ', N'Di tích khảo cổ học'),
(N'Loại di tích thiên nhiên', N'Di tích thiên nhiên độc đáo');


INSERT INTO DM_KyBaoCao (TenKyBaoCao, TrangThai, GhiChu, LoaiKyBaoCao)
VALUES 
(N'Kỳ báo cáo tháng 1', 1, N'Báo cáo đầu năm', 1),
(N'Kỳ báo cáo tháng 2', 1, N'Báo cáo tháng 2', 1),
(N'Kỳ báo cáo tháng 3', 1, N'Báo cáo tháng 3', 2),
(N'Kỳ báo cáo quý 1', 1, N'Báo cáo tổng hợp quý 1', 1),
(N'Kỳ báo cáo quý 2', 1, N'Báo cáo tổng hợp quý 2', 1),
(N'Kỳ báo cáo quý 3', 1, N'Báo cáo tổng hợp quý 3', 2),
(N'Kỳ báo cáo quý 4', 1, N'Báo cáo tổng hợp quý 4', 2),
(N'Kỳ báo cáo năm 2023', 1, N'Báo cáo tổng hợp năm 2023', 3),
(N'Kỳ báo cáo năm 2024', 0, N'Dự kiến báo cáo năm 2024', 3),
(N'Kỳ báo cáo dự án X', 1, N'Báo cáo tiến độ dự án X', 4);
--endregion

--region Insert records into  Authorization Management
-- Thêm người dùng
INSERT INTO Sys_User (UserName, Email, Password, Status, Note)
VALUES
('admin', 'admin@example.com', 'admin', 1, 'Admin user'),
('user1', 'user1@example.com', 'user1', 1, 'Regular user');

-- Thêm chức năng
INSERT INTO Sys_Function (FunctionName, Description)
VALUES
('ManageUsers', N'Quản lý người dùng'),
('ManageMonumentRanking', N'Quản lý di tích xếp hạng');

-- Thêm nhóm
INSERT INTO Sys_Group (GroupName, Description)
VALUES
('AdminGroup', N'Nhóm quản trị'),
('UserGroup', N'Nhóm người dùng');

-- Thêm người dùng vào nhóm
INSERT INTO Sys_UserInGroup (UserID, GroupID)
VALUES
(1, 1),  -- admin vào AdminGroup
(2, 2);  -- user1 vào UserGroup

-- Thêm chức năng vào nhóm và phân quyền
INSERT INTO Sys_FunctionInGroup (FunctionID, GroupID, Permission)
VALUES
(1, 1, 8),  -- AdminGroup có quyền xem, thêm, sửa (bitmask 7 = 0111)
(2, 1, 1),  -- AdminGroup có quyền xem (bitmask 1 = 0001)
(1, 2, 1),  -- UserGroup có quyền xem (bitmask 1 = 0001)
(2, 2, 7);  -- UserGroup có quyền xem, thêm, sửa (bitmask 7 = 0111)


--endregion
--endregion

--region Query
	DELETE FROM Sys_User;
	DELETE FROM Sys_Function;
	DELETE FROM Sys_Group;
	DELETE FROM Sys_UserInGroup;
	DELETE FROM Sys_FunctionInGroup;

	-- Reset giá trị IDENTITY về giá trị mặc định (1)
DBCC CHECKIDENT ('Sys_FunctionInGroup', RESEED, 0);
DBCC CHECKIDENT ('Sys_UserInGroup', RESEED, 0);
DBCC CHECKIDENT ('Sys_User', RESEED, 0);
DBCC CHECKIDENT ('Sys_Function', RESEED, 0);
DBCC CHECKIDENT ('Sys_Group', RESEED, 0);

SELECT *FROM Sys_Function sf
SELECT * FROM	Sys_User su
SELECT * FROM Sys_UserInGroup suig
SELECT * FROM	 Sys_Group sg
SELECT * FROM Sys_FunctionInGroup sfig

UPDATE Sys_User
SET Password = 'user1' WHERE UserID = 2
EXEC FIG_GetUserPermissions @UserName = 'admin', @FunctionName = 'ManageUsers' 

UPDATE Sys_FunctionInGroup 
SET  Permission = 15 WHERE FunctionInGroupID = 1
--endregion

SELECT * FROM Sys_Group sg
SELECT * FROM Sys_User su 
SELECT * FROM Sys_UserInGroup 
SELECT * FROM Sys_Function sf 
SELECT * FROM Sys_FunctionInGroup sfig 
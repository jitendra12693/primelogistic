
CREATE DATABASE [primelogisticjeetu]
GO
USE [primelogisticjeetu]
GO
/****** Object:  Table [dbo].[AdminUser]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUser](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[UniqueId] [varchar](50) NOT NULL,
	[TokenId] [varchar](150) NULL,
	[Name] [varchar](150) NOT NULL,
	[MobileNo] [varchar](10) NOT NULL,
	[EmailId] [varchar](50) NULL,
	[Username] [varchar](20) NOT NULL,
	[Password] [varchar](30) NOT NULL,
	[ReTypePassword] [varchar](30) NULL,
	[IsActive] [bit] NULL,
	[RoleId] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDt] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyDt] [datetime] NULL,
 CONSTRAINT [PK_AdminUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApiMailClient]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiMailClient](
	[ApiID] [int] NOT NULL,
	[UniqueID] [uniqueidentifier] NULL,
	[ApiUrl] [varchar](50) NULL,
	[ApiDomain] [varchar](50) NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[ApiKey] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AreaMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AreaMaster](
	[AreaId] [bigint] IDENTITY(1,1) NOT NULL,
	[AreaName] [varchar](100) NOT NULL,
	[Description] [varchar](500) NULL,
	[StateId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDt] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyDate] [datetime] NULL,
 CONSTRAINT [PK_AreaMaster] PRIMARY KEY CLUSTERED 
(
	[AreaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillDetails]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetails](
	[BillId] [bigint] IDENTITY(1,1) NOT NULL,
	[BillNo] [varchar](30) NULL,
	[PartyId] [bigint] NOT NULL,
	[PeriodFrom] [datetime] NOT NULL,
	[PeriodTo] [datetime] NOT NULL,
	[TotalAmount] [decimal](12, 2) NOT NULL,
	[FullCharges] [decimal](12, 2) NOT NULL,
	[CGST] [decimal](12, 2) NOT NULL,
	[SGST] [decimal](12, 2) NOT NULL,
	[GrandTotal] [decimal](12, 2) NOT NULL,
	[CreatedDt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[BillDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDt] [datetime] NULL,
	[BillCount] [int] NULL,
	[IsCancel] [bit] NULL,
 CONSTRAINT [PK_BillDetails] PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CountryMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryMaster](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [varchar](150) NULL,
	[Continent] [varchar](150) NULL,
	[CreatedDt] [datetime] NULL,
	[UpdatedDt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_CountryMaster] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourrierMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourrierMaster](
	[CourrierId] [bigint] IDENTITY(1,1) NOT NULL,
	[PartyId] [bigint] NOT NULL,
	[SourceId] [bigint] NULL,
	[Distance] [varchar](50) NULL,
	[DestinationId] [bigint] NULL,
	[Amount] [decimal](12, 3) NULL,
	[CourrierModeId] [int] NULL,
	[CNNo] [varchar](15) NOT NULL,
	[DepartureDt] [datetime] NOT NULL,
	[Weight] [decimal](12, 3) NOT NULL,
	[TrackingNo] [varchar](15) NULL,
	[Rate] [decimal](10, 3) NULL,
	[StatusId] [int] NULL,
	[CreatedDt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifyBy] [int] NULL,
	[ModifyDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_CourrierMaster] PRIMARY KEY CLUSTERED 
(
	[CourrierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourrierMode]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourrierMode](
	[CourrierModeId] [int] IDENTITY(1,1) NOT NULL,
	[CourrierModeName] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[IsDelete] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CourrierMode] PRIMARY KEY CLUSTERED 
(
	[CourrierModeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PartyMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PartyMaster](
	[PartyId] [bigint] IDENTITY(1,1) NOT NULL,
	[PartyName] [varchar](200) NOT NULL,
	[Address1] [varchar](200) NOT NULL,
	[Address2] [varchar](200) NULL,
	[EmailId] [varchar](50) NULL,
	[MobileNo] [varchar](12) NULL,
	[AlternateContact] [varchar](12) NULL,
	[Ladline] [varchar](12) NULL,
	[Landmark] [varchar](200) NULL,
	[City] [varchar](100) NOT NULL,
	[GSTNumber] [varchar](20) NULL,
	[FuelCharges] [decimal](5, 2) NULL,
	[StateId] [int] NOT NULL,
	[CountryId] [int] NOT NULL,
	[Pincode] [varchar](6) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDt] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyDate] [datetime] NULL,
 CONSTRAINT [PK_PartyMaster] PRIMARY KEY CLUSTERED 
(
	[PartyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMaster](
	[PaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[RecieptNo] [varchar](50) NULL,
	[PartyId] [int] NOT NULL,
	[BillId] [bigint] NOT NULL,
	[BillAmount] [decimal](18, 2) NOT NULL,
	[BalanceAmount] [decimal](18, 2) NOT NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[PaymentAmount] [decimal](18, 2) NOT NULL,
	[ChequeNo] [varchar](10) NULL,
	[BankName] [varchar](50) NULL,
	[ChequeDate] [date] NULL,
	[PaymentTypeId] [int] NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
 CONSTRAINT [PK_PaymentMaster] PRIMARY KEY CLUSTERED 
(
	[PaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMaster](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDt] [datetime] NOT NULL,
 CONSTRAINT [PK_RoleMaster] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SourceMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SourceMaster](
	[SourceId] [bigint] IDENTITY(1,1) NOT NULL,
	[SourceName] [varchar](150) NOT NULL,
	[StateId] [int] NOT NULL,
	[Description] [varchar](250) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDt] [datetime] NULL,
	[ModifyBy] [int] NULL,
	[ModifyDt] [datetime] NULL,
 CONSTRAINT [PK_SourceMaster] PRIMARY KEY CLUSTERED 
(
	[SourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StateMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateMaster](
	[StateId] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[StateName] [varchar](150) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDt] [datetime] NULL,
	[StateCode] [varchar](5) NULL,
 CONSTRAINT [PK_StateMaster] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatusMaster]    Script Date: 14-11-2022 14:53:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatusMaster](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[SubStatusId] [int] NULL,
	[StatusName] [varchar](150) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedDt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDt] [datetime] NULL,
 CONSTRAINT [PK_StatusMaster] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AdminUser] ON 
GO
INSERT [dbo].[AdminUser] ([UserId], [UniqueId], [TokenId], [Name], [MobileNo], [EmailId], [Username], [Password], [ReTypePassword], [IsActive], [RoleId], [CreatedBy], [CreatedDt], [ModifyBy], [ModifyDt]) VALUES (1, N'1', NULL, N'Prime Logistic', N'9029451159', N'info@primelogistic.co', N'admin1', N'9029451159', N'password', 1, 1, 1, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[AdminUser] OFF
GO
SET IDENTITY_INSERT [dbo].[CourrierMode] ON 
GO
INSERT [dbo].[CourrierMode] ([CourrierModeId], [CourrierModeName], [Description], [IsActive], [IsDelete], [CreatedDate], [CreatedBy], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Air', N'Air Mode', 1, 0, CAST(N'2022-11-12T20:05:29.333' AS DateTime), 1, NULL, NULL)
GO
INSERT [dbo].[CourrierMode] ([CourrierModeId], [CourrierModeName], [Description], [IsActive], [IsDelete], [CreatedDate], [CreatedBy], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Surface', N'Surface Mode', 1, 0, CAST(N'2022-11-12T20:05:53.130' AS DateTime), 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[CourrierMode] OFF
GO
ALTER TABLE [dbo].[CourrierMode] ADD  CONSTRAINT [DF_CourrierMode_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteCourrierDetails]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspDeleteCourrierDetails]
@CourrierId bigint
as
set nocount on
begin
	Update CourrierMaster set IsActive=case when IsActive= 0 then 1 else 0 end where CourrierId=@CourrierId
end
GO
/****** Object:  StoredProcedure [dbo].[uspDeletePartyDetails]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspDeletePartyDetails]
@PartyId bigint
as
--set nocount on
begin
	Update PartyMaster set IsActive=case when IsActive= 0 then 1 else 0 end where PartyId=@PartyId
end


--select * from partymaster
GO
/****** Object:  StoredProcedure [dbo].[uspDeleteSourceMaster]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[uspDeleteSourceMaster]
@SourceId bigint
as
--set nocount on
begin
	Update SourceMaster set IsActive=case when IsActive= 0 then 1 else 0 end where SourceId=@SourceId
end


--select * from partymaster
GO
/****** Object:  StoredProcedure [dbo].[uspInsertBillDetails]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspInsertBillDetails]  
@BillId bigint ,  
@PartyId bigint,  
@PeriodFrom datetime,  
@PeriodTo datetime,  
@TotalAmount decimal(12,2),  
@FullCharges decimal(12,2),  
@CGST decimal(12,2),  
@SGST decimal(12,2),  
@GrandTotal decimal(12,2),  
@UserId int,  
@GeneratedBillId varchar(50) output,  
@BillDate date output  ,
@FuelCharges decimal(10,2) output
as  
begin  
 if exists(select BillId from BillDetails where PartyId=@PartyId and CONVERT(varchar,PeriodFrom,112) =CONVERT(varchar,convert(datetime,@PeriodFrom),112)
  /*and CONVERT(varchar,PeriodTo,112) =CONVERT(varchar,convert(datetime,@PeriodTo),112) AND IsCancel<>1 */)  
 begin  
   select @BillId=BillId, @GeneratedBillId = BillNo,@BillDate= BillDate from BillDetails where PartyId=@PartyId 
    and CONVERT(varchar,PeriodFrom,112) =CONVERT(varchar,convert(datetime,@PeriodFrom),112) And IsActive=1
    /*and CONVERT(varchar,PeriodTo,112) =CONVERT(varchar,convert(datetime,@PeriodTo),112) */ 
   update BillDetails set IsActive=1,TotalAmount=@TotalAmount,FullCharges=@FullCharges,CGST=@CGST,SGST=@SGST,GrandTotal=@GrandTotal where BillId=@BillId  
   --print @BillId
   --set @GeneratedBillId=@GeneratedBillId  
 end  
 else  
 begin  
  
  declare @stateCode varchar(3)
  select @stateCode = StateCode from PartyMaster PM join StateMaster SM on SM.StateId=PM.StateId where partyid=@PartyId 

  set @BillDate=dateadd(dd,1, @PeriodTo)
	
  declare @BillNo varchar(30)   
  --select @BillNo= CONVERT(varchar(10), right(newid(),6))  
  insert into BillDetails  
  (  
   BillNo,  
   PartyId,  
   PeriodFrom,  
   PeriodTo,  
   TotalAmount,  
   FullCharges,  
   CGST,  
   SGST,  
   GrandTotal,  
   CreatedDt,  
   CreatedBy,  
   IsActive ,
   BillDate 
  )  
  values  
  (  
   null,  
   @PartyId,  
   @PeriodFrom,  
   @PeriodTo,  
   @TotalAmount,  
   @FullCharges,  
   @CGST,  
   @SGST,  
   @GrandTotal,  
   GETDATE(),  
   @UserId,  
   1  ,
   @BillDate
  )  
  select @FuelCharges=FuelCharges From PartyMaster where PartyId=@PartyId
  set @BillId=convert(bigint,@@identity)
  set @GeneratedBillId=@stateCode+convert(varchar,datepart(year,getdate()))+'00'+convert(varchar,@BillId)   
  update BillDetails set BillNo=@GeneratedBillId where BillId=@BillId
 end  
end


GO
/****** Object:  StoredProcedure [dbo].[uspInsertCourrierDetails]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspInsertCourrierDetails]
@PartyId bigint,
@SourceId bigint=1,
@DestinationId bigint=1,
@Amount decimal(12,3),
@CreatedBy int,
@TrackingNo varchar(15) output,
@CNNo varchar(15),
@Weight decimal(12,3),
@DepartureDt datetime,
@Distance varchar(50),
@Rate decimal(12,3)
as
begin
declare @Message varchar(100)
if exists(select null from CourrierMaster where CNNo=@CNNo)
begin
		
	set @TrackingNo=(select TrackingNo from CourrierMaster where CNNo=@CNNo)
	set @Message='This courier already exists with tracking bumber '+@TrackingNo
	select @TrackingNo as TrackingNo,@Message as [Message],0 as [Status]
end
else
begin

select @TrackingNo= CONVERT(varchar(10), right(newid(),10))
insert into CourrierMaster
(
PartyId,
SourceId,
DestinationId,
Amount,
CreatedDt,
CreatedBy,
IsActive,
StatusId,
TrackingNo,
CNNo,
[Weight],
DepartureDt,
Rate,
Distance
)
values
(
@PartyId,
@SourceId,
@DestinationId,
@Amount,
GETDATE(),
@CreatedBy,
1,
1,
@TrackingNo,
@CNNo,
@Weight,
@DepartureDt,
@Rate,
@Distance
)

set @Message='Your courier added successfully with tracking number '+@TrackingNo
select @TrackingNo as TrackingNo,@Message as [Message], 1 as [Status]
end
end
GO
/****** Object:  StoredProcedure [dbo].[uspInsertPayment]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[uspInsertPayment]
@PartyId int,
@BillId bigint,
@BillAmount decimal(18,2),
@BalanceAmount decimal(18,2),
@PaidAmount decimal(18,2),
@PaymentAmount decimal(18,2),
@ChequeNo varchar(10),
@BankName varchar(100),
@ChequeDate date,
@UserId int
as
begin
	
	set @BalanceAmount = @BillAmount -(@PaidAmount+@PaymentAmount)
	set @PaidAmount = @PaidAmount+@PaymentAmount
	declare @PaymentId bigint,@RecieptNo varchar(20)
	insert into PaymentMaster
	(
		PartyId,
		BillId,
		BillAmount,
		PaidAmount,
		PaymentAmount,
		BalanceAmount,
		ChequeNo,
		ChequeDate,
		BankName,
		IsActive,
		CreatedBy,
		CreatedDate
	)
	values
	(
		@PartyId,
		@BillId,
		@BillAmount,
		@PaidAmount,
		@PaymentAmount,
		@BalanceAmount,
		@ChequeNo,
		@ChequeDate,
		@BankName,
		1,
		@UserId,
		GETDATE()
	)

	set @PaymentId=SCOPE_IDENTITY()
	set @RecieptNo='IN'+convert(varchar,datepart(MONTH,getdate()))+convert(varchar,datepart(YEAR,getdate()))+convert(varchar,@PaymentId) 
	update PaymentMaster set RecieptNo=@RecieptNo where PaymentId=@PaymentId

	select @RecieptNo as ResponseId,1 as StatusId,'Success' as Status,'Your payment has been done successfully with reciept No : '+@RecieptNo as Message
end
GO
/****** Object:  StoredProcedure [dbo].[uspRegisterAdminUser]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspRegisterAdminUser]
@Name varchar(150),
@MobileNo varchar(10),
@EmailId varchar(50),
@Username varchar(20),
@Password varchar(30),
@RoleId int,
@UserId int,
@ErrorMsg varchar(50)='' output,
@ReturnUserId bigint output
as
begin
	if exists(select null from AdminUser where MobileNo=@MobileNo or Username=@Username)
	begin
		set @ErrorMsg='This user already exits'
	end
	else
	begin
		insert into AdminUser
		(
			UniqueId,
			Name,
			MobileNo,
			EmailId,
			Username,
			[Password],
			IsActive,
			RoleId,
			CreatedBy,
			CreatedDt
		)
		values
		(
			newid(),
			@Name,
			@MobileNo,
			@EmailId,
			@Username,
			@Password,
			0,
			@RoleId,
			@UserId,
			GETDATE()
		)
		set @ReturnUserId=@@IDENTITY
	end
end
GO
/****** Object:  StoredProcedure [dbo].[uspSearchCourier]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspSearchCourier] 
@PartyId bigint=null,
@CourrierId bigint,
@UserId int,
@TrackingNo varchar(15)=null,

@FromDate varchar(20),
@ToDate varchar(20),
@START_INDEX INT,  
                    
@MAX_ROWS  INT,                      
@SORT_EXPRESSION VARCHAR(40),                      
@SORT_DIRECTION VARCHAR(5)  
as
begin	
	
	declare @FuelCharges decimal(5,2)
	declare @strSql varchar(max)
	declare @sqlStringFinal varchar(max)
	declare @sqlStringCount varchar(max)
	DECLARE @sqlStringSort  varchar(4000) 
	declare @TotalAmount varchar(4000) 

	if(@PartyId is not null and @PartyId>0)
	begin
		set @FuelCharges=(select FuelCharges from PartyMaster where PartyId=@PartyId)
	end
	else
	begin 
		set @FuelCharges=20
	end
	
	IF(Len(@SORT_EXPRESSION)>0)                                                                       
      SET @sqlStringSort =' Order By '+ltrim(@SORT_EXPRESSION) +' ' +@SORT_DIRECTION                                                                        
   ELSE                                                                        
      SET @sqlStringSort =' Order By CM.DepartureDt asc'     

	SET @strSql ='select CourrierId,
	CM.PartyId,
	PM.PartyName,
	'''' as SourceName,
	CM.Distance Destination,
	CM.SourceId,
	DestinationId,
	CM.CreatedDt,
	CM.CreatedBy,
	CM.ModifyBy,
	CM.ModifyDate,
	CM.IsActive,
	StatusId,
	TrackingNo,
	CNNo,
	isnull(Rate,0) Rate,
	[Weight],
	Amount,
	(row_number() over ('+ @sqlStringSort +')) as RowRank ,
	replace(CONVERT(varchar, DepartureDt,103),'''',''-'') DepartureDt
	from CourrierMaster CM
	--join SourceMaster S on S.SourceId=CM.SourceId
	--join SourceMaster D on D.SourceId=CM.DestinationId
	join PartyMaster PM on PM.PartyId=Cm.PartyId
	where 1=1'

	if(@PartyId>0)
	begin
		set @strSql=@strSql+' AND CM.PartyId='+CONVERT(varchar,@PartyId)
	end

	if(@CourrierId>0)
	begin
		set @strSql=@strSql+' AND CM.CourrierId='+CONVERT(varchar,@CourrierId)
	end

	if(@UserId>0)
	begin
		set @strSql=@strSql+' AND CM.CreatedBy='+CONVERT(varchar,@UserId)
	end

	if(LEN(@TrackingNo)>0)
	begin
		set @strSql=@strSql+' And CNNo like''%'+@TrackingNo+'%'''
	end

	if isdate(@FromDate)=1
	begin
		set @strSql=@strSql+' And CONVERT(varchar,CM.DepartureDt,112)>='+CONVERT(varchar,convert(datetime,@FromDate),112)
	end
	if isdate(@ToDate)=1
	begin
		set @strSql=@strSql+' And CONVERT(varchar,CM.DepartureDt,112)<='+CONVERT(varchar,convert(datetime,@ToDate),112)
	end

	SET @sqlStringFinal = 'SELECT * FROM (' + @strSql + ' ) temp '                                                                        
	SET @sqlStringFinal = @sqlStringFinal + ' WHERE RowRank between ' + convert(varchar,@START_INDEX) + ' and ' + convert(varchar,@START_INDEX + @MAX_ROWS - 1)                                                                        
	--SET @sqlStringCount = 'SELECT COUNT(1) AS RecordCount FROM (' + @strSql + ') temp '
	set @TotalAmount='SELECT SUM(Amount) TotalAmount,COUNT(1) AS RecordCount,round(((SUM(Amount)+(SUM(Amount)*'+convert(varchar,@FuelCharges)+')/100)*9)/100,2) CGST,
    round(((SUM(Amount)+(SUM(Amount)*'+convert(varchar,@FuelCharges)+')/100)*9)/100,2) SGST,round((SUM(Amount)*'+convert(varchar,@FuelCharges)+')/100,2) ''Full Charges'' FROM (' + @strSql + ' ) temp'

	EXEC(@sqlStringFinal)  
	EXEC(@TotalAmount)
	print @TotalAmount
	declare @BillNumber varchar 
	select @BillNumber=convert(varchar,BillId) from BillDetails where CONVERT(varchar,PeriodFrom,112) =CONVERT(varchar,convert(datetime,@FromDate),112) and CONVERT(varchar,PeriodTo,112) =CONVERT(varchar,convert(datetime,@ToDate),112)
	select PartyId,PartyName  , isnull(PartyMaster.GSTNumber,'') GSTNumber,@BillNumber BillId,
	Address1+' '+ISNULL(Address2,'')+' '+ISNULL(Landmark,'')+' '+City+' '+SM.StateName+' '+CM.CountryName+' '+Pincode as [Address],
	convert(varchar,@FuelCharges)+ ' %' as FuelCharges
	from PartyMaster join StateMaster SM on SM.StateId=PartyMaster.StateId
    join CountryMaster CM on CM.CountryId=PartyMaster.CountryId where PartyMaster.PartyId=@PartyId 
	--EXEC(@sqlStringCount)                     
	--print @sqlStringFinal 
end
GO
/****** Object:  StoredProcedure [dbo].[uspSearchPaymentDetails]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
	
		uspSearchPaymentDetails 13
		

*/

CREATE proc [dbo].[uspSearchPaymentDetails]
@BillId bigint
as
BEGIN
	set nocount on
	select PM.PartyName,BillNo,BD.BillId,BD.GrandTotal,BD.BillDate,(BD.GrandTotal-sum(isnull(P.PaymentAmount,0))) as BalanceAmount,
	BD.GrandTotal Billamount,PM.PartyId, sum(isnull(P.PaymentAmount,0)) PaidAmount from  BillDetails BD
	left join PaymentMaster P  on BD.BillId=P.BillId
	join PartyMaster PM on PM.PartyId=BD.PartyId
	where BD.BillId=@BillId
	group by BD.BillId,PM.PartyName,BillNo,BD.GrandTotal,BD.BillDate,PM.PartyId
END
GO
/****** Object:  StoredProcedure [dbo].[uspSelectAllBill]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[uspSelectAllBill]
@PartyId int,
@SearchTerm varchar(20),
@BillDate varchar(25),
@FromDate varchar(25),
@ToDate varchar(25),
@BillId bigint
as
begin
	declare @strSql varchar(max)
	declare @FinalStrSql varchar(max)

	set @strSql='select  BillId,BillNo,convert(date,B.BillDate) BillDate ,B.PartyId,
	b.PeriodFrom,
	b.PeriodTo,
	--convert(varchar,b.PeriodFrom,103) PeriodFrom,
	--convert(varchar,b.PeriodTo,103) PeriodTo,
	B.TotalAmount,B.FullCharges,
	B.CGST,B.SGST,round(B.GrandTotal,0) GrandTotal, B.BillCount,B.IsCancel,B.IsActive,PM.PartyName,PM.GSTNumber as GSTINNumber,B.CreatedDt,
	(select isnull(sum(isnull(PaymentAmount,0)),0) from PaymentMaster where BillId=B.BillId) PaidAmount,
	 isnull((select top 1 ChequeNo from PaymentMaster where BillId=b.BillId order by PaymentId desc),'''') ChequeNo from BillDetails B
	join PartyMaster PM on PM.PartyId=B.PartyId where B.IsActive=1 and 1=1'

	if(@BillId>0)
		begin
			set @strSql=@strSql+' AND B.BillId='+CONVERT(varchar,@BillId)
		end
	if(@PartyId>0)
		begin
			set @strSql=@strSql+' AND B.PartyId='+CONVERT(varchar,@PartyId)
		end
	if(LEN(@SearchTerm)>0)
	begin
		set @strSql=@strSql+' AND  PM.PartyName like''%'+@SearchTerm+'%'' or B.BillNo like''%'+@SearchTerm+'%'' or B.BillDate like''%'+@SearchTerm+'%''' 
	end
	if(isdate(@BillDate)=1)
		begin
			set @strSql=@strSql+' AND convert(varchar,convert(date,B.BillDate))='''+convert(varchar,convert(date,@BillDate))+''''
		end
	if(ISNULL(@FromDate,'')<>'' and ISNULL(@ToDate,'')<>'')
		begin
			set @STRSQL=@STRSQL+' AND CONVERT(VARCHAR,B.BillDate,112)>='''+CONVERT(VARCHAR,CONVERT(DATETIME,@FromDate),112)+'''AND CONVERT(VARCHAR,B.BillDate,112)<='''+CONVERT(VARCHAR,CONVERT(DATETIME,@ToDate),112)+''''
		end
	
	set @FinalStrSql='select top 15  * from ('+@strSql+') temp order by CreatedDt desc'
	exec(@FinalStrSql)
	print @FinalStrSql
end
GO
/****** Object:  StoredProcedure [dbo].[uspUpdateBillDetails]    Script Date: 14-11-2022 14:53:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Proc [dbo].[uspUpdateBillDetails]
@BillId bigint,
@BillDate datetime,
@FromDate datetime,
@ToDate datetime,
@UpdatedBy int
as
begin
	set nocount on
	declare @Status int,@Message varchar(100),@TrackingNo varchar(100)
	update BillDetails set BillDate=@BillDate, PeriodFrom=@FromDate,PeriodTo=@ToDate,UpdatedDt=GETDATE(),UpdatedBy=@UpdatedBy where BillId=@BillId
	select @TrackingNo = BillNo from BillDetails where BillId=@BillId 
	set @Status=1 set @Message='Your bill details updated successfully with bill No: '+@TrackingNo
	select @Status as Status,@Message as Message,@TrackingNo As TrackingNo
end
GO
USE [master]
GO
ALTER DATABASE [primelogisticjeetu] SET  READ_WRITE 
GO

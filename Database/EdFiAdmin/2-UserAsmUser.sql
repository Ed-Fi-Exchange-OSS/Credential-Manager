CREATE TABLE [dbo].[UserAsmUser] (
    [User_UserId] INT          NOT NULL,
    [WamsId]      VARCHAR (30) NOT NULL,
    [WamsUser]    VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_UserAsmUser] PRIMARY KEY CLUSTERED ([User_UserId] ASC),
    CONSTRAINT [FK_UserAsmUser_Users] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[Users] ([UserId])
);
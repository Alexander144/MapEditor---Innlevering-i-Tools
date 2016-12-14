CREATE TABLE [dbo].[Tiles]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PositionX] INT NULL, 
    [PositionY] INT NULL, 
    [Path] VARCHAR(MAX) NULL, 
    [RotationAngle] FLOAT NULL, 
    [TopLeft] BIT NULL, 
    [TopMiddle] BIT NULL, 
    [TopRight] BIT NULL, 
    [RightMiddle] BIT NULL, 
    [BottomRight] BIT NULL, 
    [BottomMiddle] BIT NULL, 
    [BottomLeft] BIT NULL, 
    [LeftMiddle] BIT NULL
)

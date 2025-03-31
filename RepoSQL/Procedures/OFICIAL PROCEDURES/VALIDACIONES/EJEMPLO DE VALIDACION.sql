USE BD_x
GO
DROP PROCEDURE IF EXISTS dbo.VALIDATION_X
GO
CREATE PROCEDURE dbo.VALIDATION_X
@A VARCHAR(50),
@B CHAR(1) OUTPUT
AS
BEGIN
--declarecion de variables
	DECLARE @X CHAR(1) = (SELECT X FROM Tb_X
	WHERE XID = @X)

	---evaluacion de la variable
	IF(@X IS NULL)
		THROW 51000, 'X es invalida', 1;

		--resultado
	SET @B = @X
END
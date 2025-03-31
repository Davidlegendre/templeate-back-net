USE BDX;
GO
DROP PROCEDURE IF EXISTS dbo.BO_CREATE_LOG_EQUIVALENCE
GO
CREATE PROCEDURE dbo.BO_CREATE_LOG_EQUIVALENCE
@ID_ENTIDAD SMALLINT, @ID_EVENTO SMALLINT,
@MENSAJE VARCHAR(MAX),
@IDENTIFICADOR VARCHAR(50)
AS
BEGIN
	---SI ES DE TIPO CANCELADO = 3 SE VA A BO_EMAILS_SEND_LOG
	INSERT INTO dbo.BO_LOG(ID_ENTIDAD, ID_EVENTO, MENSAJE, IDENTIFICADOR)
	VALUES (@ID_ENTIDAD, @ID_EVENTO, @MENSAJE, @IDENTIFICADOR)

	---ELIMINARA TODO DEL IDENTIFICADOR	DE ESA ENTIDAD
	---POR SI ACASO SE QUIERA HACER UNA TABLA PARA ENVIAR CORREOS DE LOGS
	DELETE FROM dbo.BO_EMAILS_SEND_LOG WHERE IDENTIFICADOR = @IDENTIFICADOR AND ID_ENTIDAD = @ID_ENTIDAD
	IF(@ID_EVENTO = 3)--CANCELADO
	BEGIN 
		DECLARE @IDREG BIGINT = SCOPE_IDENTITY();
		DECLARE @ENTIDAD_NOMBRE VARCHAR(30) = (SELECT DESCRIPCION FROM BO_ENTIDAD_LOG WHERE ID = @ID_ENTIDAD)

		INSERT INTO dbo.BO_EMAILS_SEND_LOG(ID_LOG, ID_ENTIDAD, ENTIDAD_NOMBRE, IDENTIFICADOR, MENSAJE)
		VALUES(@IDREG, @ID_ENTIDAD, @ENTIDAD_NOMBRE, @IDENTIFICADOR, @MENSAJE)
	END
END
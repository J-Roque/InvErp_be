-- Script de inicialización de Base de Datos para InvERP
-- Una sola base de datos para todos los microservicios

USE master;
GO

-- Crear base de datos Inventech si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Inventech')
BEGIN
    CREATE DATABASE Inventech;
    PRINT 'Base de datos Inventech creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Base de datos Inventech ya existe';
END
GO

PRINT 'Inicialización de base de datos Inventech completada';
GO

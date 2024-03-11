CREATE OR ALTER PROCEDURE dbo.GetUserById
    @role NVARCHAR(50),
    @id NVARCHAR(50)
AS
BEGIN
    IF @role = 'patient'
    BEGIN
        EXECUTE AS USER
        = 'patient';
    END
    ELSE
    BEGIN
        EXECUTE AS USER
        = 'helpdesk';
    END
    

    SELECT 
        [dbo].[Patients].Id,
        [dbo].[Patients].FullName, 
        [dbo].[AspNetUsers].Email,
        [dbo].[AspNetUsers].[PhoneNumber],
        [dbo].[MedicalRecords].DiagnosisDetails, 
        [dbo].[MedicalRecords].MedicalRecordNumber, 
        [dbo].[MedicalRecords].TreatmentPlan
    FROM [dbo].[Patients] INNER JOIN [dbo].[MedicalRecords] ON
        [dbo].[Patients].MedicalRecordId = [dbo].[MedicalRecords].Id
        INNER JOIN [dbo].[AspNetUsers] ON
        [dbo].[Patients].[Id] = [dbo].[AspNetUsers].[Id]
        WHERE [dbo].[AspNetUsers].[Id] = @id
    REVERT;
END;
GO



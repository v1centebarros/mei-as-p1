CREATE OR ALTER PROCEDURE dbo.GetUserData
    @role NVARCHAR(50)
AS
BEGIN
    IF @role = 'client'
    BEGIN
        EXECUTE AS USER
        = 'client';
    END
    ELSE
    BEGIN
        EXECUTE AS USER
        = 'helpdesk';
    END
    SELECT 
    dbo.AspNetUsers.Id,dbo.AspNetUsers.FullName, dbo.AspNetUsers.Email, dbo.AspNetUsers.PhoneNumber, dbo.MedicalRecords.TreatmentPlan, dbo.MedicalRecords.DiagnosisDetails, dbo.MedicalRecords.AccessCode
     FROM dbo.AspNetUsers
     INNER JOIN dbo.MedicalRecords ON dbo.AspNetUsers.MedicalRecordId = dbo.MedicalRecords.Id;
    REVERT;
END;
GO



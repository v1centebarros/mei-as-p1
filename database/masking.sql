USE merda;
ALTER TABLE dbo.AspNetUsers
ALTER COLUMN PhoneNumber ADD MASKED WITH (FUNCTION = 'partial(2,"xxxx",0)');
ALTER TABLE dbo.MedicalRecords
ALTER COLUMN TreatmentPlan ADD MASKED WITH (FUNCTION = 'partial(2,"xxxx",0)');

ALTER TABLE dbo.MedicalRecords
ALTER COLUMN DiagnosisDetails ADD MASKED WITH (FUNCTION = 'partial(2,"xxxx",0)');

CREATE USER helpdesk WITHOUT LOGIN;

CREATE USER patient WITHOUT LOGIN;

ALTER ROLE db_datareader ADD MEMBER helpdesk;

ALTER ROLE db_datareader ADD MEMBER patient;

GRANT UNMASK ON  dbo.MedicalRecords(DiagnosisDetails) TO patient;

GRANT UNMASK ON  dbo.AspNetUsers(PhoneNumber) TO patient;

GRANT UNMASK ON  dbo.MedicalRecords(TreatmentPlan) TO patient;



-- EXECUTE AS USER = 'patient';
-- SELECT * FROM dbo.AspNetUsers;
-- REVERT;

-- GRANT UNMASK ON  dbo.AspNetUsers(PhoneNumber) TO patient;
-- EXECUTE AS USER = 'helpdesk';
-- SELECT * FROM dbo.AspNetUsers;
-- REVERT;

--Run Stored Procedure as patient
EXEC dbo.GetUserData @role = 'patient';

--Run Stored Procedure as helpdesk

EXEC dbo.GetUserData @role = 'helpdesk';


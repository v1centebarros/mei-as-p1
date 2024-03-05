USE merda;
ALTER TABLE dbo.AspNetUsers
ALTER COLUMN PhoneNumber ADD MASKED WITH (FUNCTION = 'partial(2,"xxxx",0)');
ALTER TABLE dbo.MedicalRecords
ALTER COLUMN TreatmentPlan ADD MASKED WITH (FUNCTION = 'partial(2,"xxxx",0)');

ALTER TABLE dbo.MedicalRecords
ALTER COLUMN DiagnosisDetails ADD MASKED WITH (FUNCTION = 'partial(2,"xxxx",0)');

CREATE USER helpdesk WITHOUT LOGIN;

CREATE USER client WITHOUT LOGIN;

ALTER ROLE db_datareader ADD MEMBER helpdesk;

ALTER ROLE db_datareader ADD MEMBER client;

GRANT UNMASK ON  dbo.MedicalRecords(DiagnosisDetails) TO client;

GRANT UNMASK ON  dbo.AspNetUsers(PhoneNumber) TO client;



EXECUTE AS USER = 'client';
SELECT * FROM dbo.AspNetUsers;
REVERT;

GRANT UNMASK ON  dbo.AspNetUsers(PhoneNumber) TO client;
EXECUTE AS USER = 'helpdesk';
SELECT * FROM dbo.AspNetUsers;
REVERT;

--Run Stored Procedure as client
EXEC dbo.GetUserData @role = 'client';

--Run Stored Procedure as helpdesk

EXEC dbo.GetUserData @role = 'helpdesk';


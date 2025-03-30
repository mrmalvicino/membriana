use membriana_db;

SET
    QUOTED_IDENTIFIER ON;

go
------------
-- IMAGES --
------------
print '';

print 'Inserting dummy data into Images table...';

go
insert into
    Images (Url)
values
    ('https://i.imgur.com/Cy1SqZy.png'),
    ('https://i.imgur.com/zeJG0nu.jpeg');

go
-------------------
-- ORGANIZATIONS --
-------------------
print '';

print 'Inserting dummy data into Organizations table...';

go
insert into
    Organizations (
        Active,
        Name,
        Email,
        Phone,
        LogoImageId,
        PricingPlanId
    )
values
    (
        1,
        'Ftnes Gym',
        'ftnesgym@mail.com',
        '1512345678',
        1,
        1
    );

go
------------
-- PEOPLE --
------------
print '';

print 'Inserting dummy data into People table...';

go
insert into
    People (
        Active,
        Name,
        Email,
        Phone,
        Dni,
        BirthDate,
        ProfileImageId
    )
values
    (
        1,
        'Berlinguieri, Carlos',
        'berlinc@mail.com',
        '1512341234',
        '30123456',
        '1990-12-12',
        2
    );

go
---------------
-- EMPLOYEES --
---------------
print '';

print 'Inserting dummy data into Employees table...';

go
insert into
    Employees (Id, AdmissionDate, OrganizationId)
values
    (1, '2025-03-25', 1);
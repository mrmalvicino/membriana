use membriana_db;

go
-------------------
-- PRICING PLANS --
-------------------
print '';

print 'Inserting initial data into PricingPlans table...';

go
insert into
    PricingPlans (Name, Fee)
values
    ('Plan gratuito', 0),
    ('Plan profesional', 20),
    ('Plan empresarial', 30);
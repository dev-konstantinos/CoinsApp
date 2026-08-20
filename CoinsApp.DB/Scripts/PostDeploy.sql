/*
    CoinsApp Post-Deployment Script
    Global seed only. Development test data is NOT executed here.
*/

:r .\Seed\Global\Countries.sql
:r .\Seed\Global\Currencies.sql
:r .\Seed\Global\Materials.sql
:r .\Seed\Global\Mints.sql
:r .\Seed\Global\IssuingAuthorities.sql
:r .\Seed\Global\Denominations.sql
:r .\Seed\Global\Catalogs.sql
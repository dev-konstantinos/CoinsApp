/*
    CoinsApp Post-Deployment Script

    Global seed only. Development test data is NOT executed here.
*/

:r ..\Seed\Global\Countries.sql
GO

:r ..\Seed\Global\Currencies.sql
GO

:r ..\Seed\Global\CountryCurrencies.sql
GO

:r ..\Seed\Global\Materials.sql
GO

:r ..\Seed\Global\Mints.sql
GO

:r ..\Seed\Global\Denominations.sql
GO

:r ..\Seed\Global\Catalogs.sql
GO
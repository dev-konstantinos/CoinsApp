CREATE UNIQUE INDEX [UX_Countries_IsoCode]
ON [dbo].[Countries] ([IsoCode])
WHERE [IsoCode] IS NOT NULL;
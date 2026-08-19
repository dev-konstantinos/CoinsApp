CREATE UNIQUE INDEX [UX_Users_Email]
ON [dbo].[Users] ([Email])
WHERE [Email] IS NOT NULL;
﻿ALTER TABLE [dbo].[Nop_Language]
    ADD CONSTRAINT [Nop_Language_PK] PRIMARY KEY CLUSTERED ([LanguageId] ASC) WITH (FILLFACTOR = 80, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

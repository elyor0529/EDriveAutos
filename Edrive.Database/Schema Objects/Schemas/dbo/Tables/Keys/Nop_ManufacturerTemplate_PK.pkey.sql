﻿ALTER TABLE [dbo].[Nop_ManufacturerTemplate]
    ADD CONSTRAINT [Nop_ManufacturerTemplate_PK] PRIMARY KEY CLUSTERED ([ManufacturerTemplateId] ASC) WITH (FILLFACTOR = 80, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

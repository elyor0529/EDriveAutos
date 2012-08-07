ALTER TABLE [dbo].[Nop_Affiliate]
    ADD CONSTRAINT [DF_Nop_Affiliate_Active] DEFAULT ((1)) FOR [Active];


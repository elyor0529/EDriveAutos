﻿ALTER TABLE [dbo].[Nop_Log]
    ADD CONSTRAINT [FK_Nop_Log_Nop_LogType] FOREIGN KEY ([LogTypeID]) REFERENCES [dbo].[Nop_LogType] ([LogTypeID]) ON DELETE CASCADE ON UPDATE CASCADE;

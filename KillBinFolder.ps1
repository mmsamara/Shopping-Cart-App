﻿Get-Process | Where-Object -Property Name -EQ 'VBCSCompiler' | Stop-Process -Force -Verbose
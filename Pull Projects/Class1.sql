
	SELECT 
    CAST(
        DATEADD(HOUR, 
            DATEPART(HOUR, MojaData AT TIME ZONE 'Central European Standard Time') 
            - DATEPART(HOUR, SWITCHOFFSET(MojaData AT TIME ZONE 'Central European Standard Time', 0)), 
            MojaData AT TIME ZONE 'Central European Standard Time'
        ) AS datetime
    ) AS AdjustedTime
FROM 
    [MyDatabase].[dbo].[Orders];

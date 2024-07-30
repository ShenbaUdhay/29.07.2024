  
  
Alter PROCEDURE [dbo].[Invoice_EDDExport_SP]      
  --Declare  
@EDDID As  NVARCHAR(max) = ''    
,@JobID as nvarchar(max)=''    
--drop table #TempOrderID    
AS        
BEGIN     
  
    
SELECT DISTINCT         
        
 * From         
 (        
         SELECT *,        
                              ROW_NUMBER() OVER ( ORDER BY Subquery1.[Invoice/CM #] ASC ) AS [Invoice/CMDistribution] ,        
                              COUNT(*) OVER () AS [NumberOfDistributions]                                  
          
                  FROM        
      ( SELECT DISTINCT         
        
              
                                             cus.ClientCode As CustomerID,        
                                             ed.EDDID,        
                                             cus.CustomerName AS ClientName,               
                                             (SELECT TOP 1 i5.InvoiceID from Invoicing i5        
                                             INNER JOIN Samplecheckin sc1 On i5.JobID= sc1.JobID         
                                             WHERE i5.GCRecord IS NULL        
                                             AND i5.JobID = i.JobID) AS [Invoice/CM #],              
                                             CONVERT(NVARCHAR(10), 'False') AS [CreditMemo] ,        
                                             FORMAT(i.QuotedDate, 'MM:dd:yyyy HH:mm') AS [Date],        
                                             ''AS [Quote] ,        
                                             qu.QuoteID  AS [Quote#] ,        
                                             '' AS [QuoteGoodThruDate] ,         
                                             CONVERT(NVARCHAR(10), 'False') AS DropShip,        
                                             cus.CustomerName [ShipToName],        
                                             qu.ShipStreet1 AS ShipToAddressLineOne,        
                                             qu.ShipStreet1 AS ShipToAddressLineTwo,        
                                             qu.ShipCity AS ShipToCity,        
                                             qu.ShipZipCode AS ShipToZipCode,         
                                             qu.ShipState AS ShipToState,        
                                             qu.ShipCountry AS ShipToCountry,           
                                             i.PO AS CustomerPO,            
                                             CASE When sv.[option] IS NOT NULL        
                                             And LEN(sv.[option])>0        
                                             Then sv.[option]        
                                             Else  'Airborne'        
                                             END As [ShipVia] ,          
                                             FORMAT( i.CreatedDate, 'MM:dd:yyyy HH:mm')AS[ShipDate],        
                                             FORMAT(sl.DueDate, 'MM:dd:yyyy HH:mm')AS[DateDue],        
                                             i.DiscountAmount,         
                                             FORMAT(qu.QuotedDate, 'MM:dd:yyyy HH:mm')  As [DiscountDate],        
                                             CASE WHEN dp.Term IS NOT NULL        
                                             AND LEN(dp.Term) > 0        
                                             THEN dp.Term        
                                             ELSE 'Net 30 Days'        
                                             END AS DisplayedTerms ,        
                                             ( SELECT TOP 1   u.CustomerName        
                                             FROM      CRMQuotes q        
                                             INNER JOIN Customer u ON u.Oid = q.Client        
                                WHERE     q.Oid = sc.QuoteID        
                                             ) AS [SalesRepresentativeID] ,        
                                             CASE When dp.DepositID IS NOT NULL        
                                             And LEN(dp.DepositID)>0        
                                             Then dp.DepositID        
                                             Else '12000'        
                                             END AS [AccountsReceivableAccount],         
                                             i.Amount AS [AccountsReceivableAmount],        
                                            '' AS [InvoiceNote] ,        
                                            '0' AS [ApplyToInvoiceDistribution] ,     
                                            --(SELECT TOP 1  n.Title from Notes n        
                                            --inner join Invoicing sc on sc.Oid=n.Invoicing)  AS [InvoiceNote] ,        
                                            iac.Qty As Quantity,        
                                            vm.VisualMatrixName AS ItemID,     
                                            '' AS [SerialNumber] ,  
                                            iac.[Description] AS[Description],        
                                                
                                            (SELECT TOP 1  tps.PriceCode FROM Invoicing i1         
                                             inner join Samplecheckin sc on sc.JobID  =i.JobID         
                                             inner join SampleLogIn sl on sl.JobID  =sc.Oid         
                                             inner join SampleParameter sp on sp.Samplelogin =sl.Oid          
                                             inner join Testparameter tp on tp.Oid =sp.Testparameter         
                                             INNER JOIN TestMethod tpd ON tp.TestMethod =tpd.Oid         
                                             INNER JOIn TurnAroundTime tat On tat.Oid=sc.TAT        
                                             inner join TestPriceSurcharge tps on tpd.Oid  = tps.Test  where  i1.JobID=i.JobID And tat.Oid=sc.TAT) AS [G/LAccount],                                                           
                                            (select TOP 1  iic.UnitPrice    from Invoicing i        
                                             Inner join InvoicingAnalysisCharge iic on i.Oid=iic.Invoice        
                                             Inner join TurnAroundTime ta  on ta.Oid=i.TAT        
                                             Inner join TestMethod tm on iic.Test=tm.Oid        
                                             And (iic.Test= tm.Oid or        
                                             iic.Test IS NULL)        
                                             And (iic.TAT=ta.Oid or        
                                             iic.TAT IS NULL )        
                                             ) AS [UnitPrice],        
                                             (select Top 1 po.Tax  from Purchaseorder po        
                                             inner join Requisition rq on rq.Oid=po.Vendor) AS [TaxType],        
                                              i.Amount *-1 As Amount,                                                    
                                             '' AS [InventoryAccount] ,                       
                                             '' AS [CostOfSalesAccount] ,      
                                             i.JobID,     
                                             '' AS [SalesTaxAgencyID] ,         
                                             '0' AS [RecurNumber],         
                                             '0' AS [RecurFrequency]                                  
                               FROM        
                                         Invoicing i       
                                         inner join Customer cus on cus.Oid=i.Client          
                                         inner Join InvoicingEDDExport ed on ed.InvoiceID=i.Oid        
                                         inner join InvoicingAnalysisCharge iac on i.Oid=iac.Invoice and iac.GCRecord is null        
                                         inner join Samplecheckin sc on sc.JobID=i.JobID and sc.GCRecord is null        
                                         inner join SampleLogIn sl on sl.JobID =sc.Oid and sl.GCRecord is null        
                                         --left join InvoicingItemCharge iic on iic.Invoicing =i.Oid            
                                         ----and iic.GCRecord is null        
                                         left join Notes n on sc.Oid =n.Samplecheckin 
										 left Join CRMQuotes qu on i.QuoteID=qu.Oid and i.GCRecord is null        
										 left join AnalysisPricing ap on qu.Oid=ap.CRMQuotes  and qu.GCRecord is null          
                                         left join TestMethod tm on tm.Oid=sc.Test        
                                         left join Method m on m.Oid=tm.MethodName         
                                         left join VisualMatrix vm on vm.Oid=sl.VisualMatrix        
                                         left Join Matrix mt on mt.Oid=vm.MatrixName        
                                         left Join Items it On it.Matrix= mt.Oid        
                                         left join Vendors vd on vd.Oid= it.Vendor        
                                         left join Requisition rq on vd.Oid=rq.Vendor        
                                         left Join Purchaseorder po on po.Oid=rq.POID        
                                         Left join ShipVia sv on sv.Oid= po.ShipVia        
                                         left join Deposits dp on i.Oid=dp.InvoiceID        
                                         Left join TestPriceSurcharge ts on tm.Oid=ts.Test        
                              WHERE        
            
                                         i.GCRecord IS NULL       
     
                                          And  ed.EDDID IN(SELECT value FROM STRING_SPLIT(@EDDID , ','))      
                                          and i.JobID IN (SELECT value FROM STRING_SPLIT(@JobID, ','))                                      
                     ) AS Subquery1        
           
        
 UNION        
        
     SELECT    * ,        
                                 ROW_NUMBER() OVER (ORDER BY Result2.[Invoice/CM #] ASC ) AS [Invoice/CMDistribution] ,        
                                 COUNT(*) OVER () AS [NumberOfDistributions]        
                        FROM( SELECT DISTINCT        
                                    cus.ClientCode As CustomerID,        
                                    ed.EDDID,        
                                    cus.CustomerName AS ClientName,        
                                    i.InvoiceID AS  [Invoice/CM #],        
                                    CONVERT(NVARCHAR(10), 'False') AS [CreditMemo] ,        
                                    FORMAT(qu.QuotedDate, 'MM:dd:yyyy HH:mm') AS [Date],        
                                    ''AS [Quote] ,        
                                    qu.QuoteID  AS [Quote#] ,        
                                    '' AS [QuoteGoodThruDate] ,        
                                    CONVERT(NVARCHAR(10), 'False') AS DropShip,        
                                    cus.CustomerName AS [ShipToName],        
                                    qu.ShipStreet1 AS ShipToAddressLineOne,        
                                    qu.ShipStreet1 AS ShipToAddressLineTwo,        
                                    qu.ShipCity AS ShipToCity,        
                                    qu.ShipZipCode AS ShipToZipCode,        
                                    qu.ShipState AS ShipToState,        
                                    qu.ShipCountry AS ShipToCountry,        
                                    i.PO AS CustomerPO,        
                                   CASE When sv.[option] IS NOT NULL        
                                          And LEN(sv.[option])>0        
                                          Then sv.[option]        
                                          Else  'Airborne'        
                                   END As [ShipVia] ,         
                                   FORMAT( i.CreatedDate, 'MM:dd:yyyy HH:mm')AS[ShipDate],        
                                   FORMAT(sl.DueDate, 'MM:dd:yyyy HH:mm')AS[DateDue],        
                                   i.DiscountAmount,         
                                   FORMAT(qu.QuotedDate, 'MM:dd:yyyy HH:mm')  As [DiscountDate],        
                                   CASE WHEN dp.Term IS NOT NULL        
                                        AND LEN(dp.Term) > 0        
                                        THEN dp.Term        
                                        ELSE 'Net 30 Days'        
                                   END AS DisplayedTerms ,        
                                   ( SELECT TOP 1   u.CustomerName        
                                          FROM      CRMQuotes q        
                                          INNER JOIN Customer u ON u.Oid = q.Client        
                                          WHERE     q.Oid = sc.QuoteID        
                                        ) AS [SalesRepresentativeID] ,        
                                   CASE When dp.DepositID IS NOT NULL        
                                           And LEN(dp.DepositID)>0        
                                           Then dp.DepositID        
                                           Else '12000'        
                                   END AS [AccountsReceivableAccount],                                          
                                  i.Amount AS [AccountsReceivableAmount],        
                                  '' AS [InvoiceNote] ,        
                                  '0' AS [ApplyToInvoiceDistribution] ,   
                                  iac.Qty As Quantity,        
                                  vm.VisualMatrixName AS ItemID,    
                                  '' AS [SerialNumber] ,  
                                  iac.[Description] AS[Description],                                                  
                                  (SELECT TOP 1  tps.PriceCode FROM Invoicing i1         
                                         INNER JOIN Samplecheckin sc on sc.JobID  =i.JobID         
                                         INNER JOIN SampleLogIn sl on sl.JobID  =sc.Oid         
                                         INNER JOIN SampleParameter sp on sp.Samplelogin =sl.Oid          
                                         INNER JOIN Testparameter tp on tp.Oid =sp.Testparameter         
                                         INNER JOIN TestMethod tpd ON tp.TestMethod =tpd.Oid         
                                         inner join TestPriceSurcharge tps on tpd.Oid  = tps.Test  where  i1.JobID=i.JobID ) AS [G/LAccount],                         
                                  (select TOP 1  iic.UnitPrice    from Invoicing i        
                                         Inner join InvoicingAnalysisCharge iic on i.Oid=iic.Invoice        
                                         Inner join TurnAroundTime ta  on ta.Oid=i.TAT        
                                         Inner join TestMethod tm on iic.Test=tm.Oid                   
                                   ) AS [UnitPrice],        
                                    (select Top 1 po.Tax  from Purchaseorder po        
                                         inner join Requisition rq on rq.Oid=po.Vendor) AS [TaxType],        
                                         i.Amount *-1 As Amount,   
                                         '' AS [InventoryAccount] ,         
                                         '' AS [CostOfSalesAccount] ,      
                                         i.JobID,       
                                         '' AS [SalesTaxAgencyID] ,         
                                         '0' AS [RecurNumber] ,         
                                         '0' AS [RecurFrequency]                            
                FROM        
                                         Invoicing i    
                                         inner join Customer cus on cus.Oid=i.Client          
                                         inner Join InvoicingEDDExport ed on ed.InvoiceID=i.Oid        
                                         inner join InvoicingAnalysisCharge iac on i.Oid=iac.Invoice and iac.GCRecord is null        
                                         inner join Samplecheckin sc on sc.JobID=i.JobID and sc.GCRecord is null        
                                         inner join SampleLogIn sl on sl.JobID =sc.Oid and sl.GCRecord is null        
                                         left join InvoicingItemCharge iic on iic.Invoicing =i.Oid            
                                         ----and iic.GCRecord is null        
                                         left join Notes n on sc.Oid =n.Samplecheckin    
										 left Join CRMQuotes qu on i.QuoteID=qu.Oid and i.GCRecord is null       
										 left join AnalysisPricing ap on qu.Oid=ap.CRMQuotes  and qu.GCRecord is null          

                                         left join TestMethod tm on tm.Oid=sc.Test        
                                         left join Method m on m.Oid=tm.MethodName         
                                         left join VisualMatrix vm on vm.Oid=sl.VisualMatrix        
                                         left Join Matrix mt on mt.Oid=vm.MatrixName        
                                         left Join Items it On it.Matrix= mt.Oid        
                                         left join Vendors vd on vd.Oid= it.Vendor        
                                         left join Requisition rq on vd.Oid=rq.Vendor        
                                         left Join Purchaseorder po on po.Oid=rq.POID        
                                         Left join ShipVia sv on sv.Oid= po.ShipVia        
                                         left join Deposits dp on i.Oid=dp.InvoiceID        
                                         Left join TestPriceSurcharge ts on tm.Oid=ts.Test        
         
                              WHERE  i.GCRecord IS NULL     
                              --And i.JobID=i.JobID        
             
     ) AS Result2       
       
                                            
    
 where      
        Result2.EDDID IN(SELECT value FROM STRING_SPLIT(@EDDID , ','))      
        and Result2.JobID IN (SELECT value FROM STRING_SPLIT(@JobID, ','))    
  
     ) AS Result       
      
   
END;   
  

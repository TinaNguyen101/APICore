SELECT A.Id 'ProjectId' 
                        , Customer.CustName 
                         , A.ProjectName 
                         , A.ProjectDecription 
                         , A.StartDate 'ProjectStartDate' 
                         , A.EndDate 'ProjectEndDate' 
                         , A.EstimateCost 'ProjectEstimateCost' 
                         , MstCostCurrency.CostCurrencySymboy 'ProjectCurrencySymboy' 
                         , A.DeliveryDate 'ProjectDeliveryDate' 
                         , A.PaymentDate 'ProjectPaymentDate' 
                         , MstProjectStatus.ProjectStatusName 'ProjectStatus' 
						 , CASE
								WHEN A.EstimateCost is null  THEN 0 + A.MaintenanceEstimateCost
								WHEN A.MaintenanceEstimateCost is null  THEN  A.EstimateCost + 0
								ELSE A.EstimateCost + A.MaintenanceEstimateCost
							END 'ProjectTotalEstimateCost'
						 ,leader.EmpName 'ProjectLeader'
						 ,coder.EmpName 'ProjectCoder'
						  ,tester.EmpName 'ProjectTester'
						 
                         FROM (select Project.Id
								, Project.CustId 
								, Project.ProjectName 
								, Project.ProjectDecription 
								, Project.StartDate 
								, Project.EndDate 
								, Project.EstimateCost 
								, Project.EstimateCostCurrencyId 
								, Project.DeliveryDate 
								, Project.PaymentDate 
								, Project.ProjectStatusId 
						  ,sum(ProjectMaintenance.EstimateCost) as MaintenanceEstimateCost
						  ,Max(ProjectMaintenance.StartDate) as MaintenanceStartDate
						  ,Max(ProjectMaintenance.EndDate) as MaintenanceEndDate
						  ,Max(ProjectMaintenance.MaintenanceStatusId) as MaintenanceStatusId
						  from Project left join ProjectMaintenance on Project.Id = ProjectMaintenance.ProjectId 
								group by Project.Id
								, Project.CustId 
								, Project.ProjectName 
								, Project.ProjectDecription 
								, Project.StartDate 
								, Project.EndDate 
								, Project.EstimateCost 
								, Project.DeliveryDate 
								, Project.PaymentDate 
								, Project.ProjectStatusId
								-- ,  ProjectMaintenance.StartDate 
						  --,  ProjectMaintenance.EndDate 
						  --,  ProjectMaintenance.MaintenanceStatusId
						 ) as A
                         left join Customer on A.CustId = Customer.Id 
                         left join MstCostCurrency on A.EstimateCostCurrencyId = MstCostCurrency.Id 
                         left join MstProjectStatus on A.ProjectStatusId = MstProjectStatus.Id
                         
						 left join   (	SELECT ProjectId , ProjectPositionId ,																			
											STUFF((	SELECT ', ' + A.EmpName 
													FROM (select Member.*, Employee.EmpName from Member left join Employee on Member.EmpId = Employee.Id WHERE 	ProjectPositionId = 1	)  A															
													Where A.ProjectId=B.ProjectId FOR XML PATH('')
												 )
												 ,1,1,'') As EmpName														
										 From (select Member.*, Employee.EmpName from Member left join Employee on Member.EmpId = Employee.Id)  B		
										 											
										 Group By ProjectId ,ProjectPositionId  															
									) leader on A.Id = leader.ProjectId 	
									
						 left join   (	SELECT ProjectId , ProjectPositionId ,																			
											STUFF((	SELECT ', ' + A.EmpName 
													FROM (select Member.*, Employee.EmpName from Member left join Employee on Member.EmpId = Employee.Id WHERE 	ProjectPositionId = 2	)  A															
													Where A.ProjectId=B.ProjectId FOR XML PATH('')
												 )
												 ,1,1,'') As EmpName														
										 From (select Member.*, Employee.EmpName from Member left join Employee on Member.EmpId = Employee.Id)  B		
										 											
										 Group By ProjectId ,ProjectPositionId  															
									) coder on A.Id = coder.ProjectId 																
						 left join   (	SELECT ProjectId , ProjectPositionId ,																			
											STUFF((	SELECT ', ' + A.EmpName 
													FROM (select Member.*, Employee.EmpName from Member left join Employee on Member.EmpId = Employee.Id WHERE 	ProjectPositionId = 3	)  A															
													Where A.ProjectId=B.ProjectId FOR XML PATH('')
												 )
												 ,1,1,'') As EmpName														
										 From (select Member.*, Employee.EmpName from Member left join Employee on Member.EmpId = Employee.Id)  B		
										 											
										 Group By ProjectId ,ProjectPositionId  															
									) tester  on A.Id = tester.ProjectId 	
									  WHERE ( '201906'  >= FORMAT(A.StartDate, 'yyyyMM') 
                                  AND '201906' <= FORMAT(A.EndDate, 'yyyyMM')) 
                                   OR('201906' >= FORMAT(A.MaintenanceStartDate, 'yyyyMM') 
                                   AND '201906' <= FORMAT(A.MaintenanceEndDate, 'yyyyMM'))
								   --and A.ProjectStatusId = 1
           --              OR A.MaintenanceStatusId =  1
						 Group By A.Id
										 , Customer.CustName 
										 , A.ProjectName 
										 , A.ProjectDecription 
										 , A.StartDate 
										 , A.EndDate 
										 , A.EstimateCost 
										 , MstCostCurrency.CostCurrencySymboy 
										 , A.DeliveryDate
										 , A.PaymentDate 
										 , MstProjectStatus.ProjectStatusName 
										 , leader.EmpName 
										 , coder.EmpName 
										 , tester.EmpName 
										 , A.EstimateCost
										 , A.MaintenanceEstimateCost

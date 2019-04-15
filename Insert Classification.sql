Insert into CLASSIFICATIONS (Id,NAME,NAMEENG,IsHierachy,REMARKS,isDeleted) values ('KID',N'Класификация на икономическите дейности','Statistical Classification of Economic Activities (NACE)','true',null,'false');
Insert into CLASSIFICATIONS (Id,NAME,NAMEENG,IsHierachy,REMARKS,isDeleted) values ('NORESP_REASONS',N'Кодова таблица на възможни причини за непопълване на анкетна карта','Reasons for "No Response Card"','false',null,'false');
Insert into CLASSIFICATIONS (Id,NAME,NAMEENG,IsHierachy,REMARKS,isDeleted) values ('EMP_CNT_GROUPS',N'Групи по брой наети лица','Groups by employee count','false',null,'false');

Insert into ClassVersions (CLASSIF,VERSION,VALID_FROM,VALID_TO,REMARKS,IsDeleted) values ('KID',2003,'2003-01-01','2007-12-31',null,'false');
Insert into ClassVersions (CLASSIF,VERSION,VALID_FROM,VALID_TO,REMARKS,IsDeleted) values ('KID',2008,'2008-01-01',null,null,'false');
Insert into ClassVersions (CLASSIF,VERSION,VALID_FROM,VALID_TO,REMARKS,IsDeleted) values ('NORESP_REASONS',1,'2018-01-01',null,null,'false');
Insert into ClassVersions (CLASSIF,VERSION,VALID_FROM,VALID_TO,REMARKS,IsDeleted) values ('EMP_CNT_GROUPS',1,'2018-01-01',null,null,'false');

Insert into ClassRelationsTypes(Id,DESCRIPTION,SrcClassifId,DestClassifId, IsDeleted) values ('NKID_KID',N'Преход от НКИД-2003 към КИД-2008','KID','KID','false');


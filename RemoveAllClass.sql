use DB_A46C98_class


delete from ClassRelations
delete from ClassRelationsTypes
ALTER TABLE ClassItems NOCHECK CONSTRAINT FK_ClassItems_ClassItems_Classif_Version_ParentItemCode
delete from classitems
ALTER TABLE ClassItems CHECK CONSTRAINT FK_ClassItems_ClassItems_Classif_Version_ParentItemCode
delete from ClassVersions
delete from Classifications
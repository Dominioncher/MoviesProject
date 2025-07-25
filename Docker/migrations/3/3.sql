create table objects_store
( 
  id raw(16) default sys_guid() primary key,
  obj BLOB
)
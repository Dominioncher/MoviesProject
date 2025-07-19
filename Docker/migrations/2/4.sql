CREATE OR REPLACE PROCEDURE VIDEO_RENT.DELETE_MOVIE (p_movie_id NUMBER, p_user_oid VARCHAR2) 
AS  
BEGIN	
	UPDATE VIDEO_RENT.MOVIES
	SET 
	deleted_date = SYSDATE, 
	deleted_by = p_user_oid
	WHERE ID = p_movie_id;	
END;
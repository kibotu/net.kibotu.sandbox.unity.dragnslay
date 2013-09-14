SCRIPT_PATH=$( (cd -P $(dirname $0) && pwd) )

RSYNC_CMD="rsync -aCx --progress --exclude-from 'copy_ignore/copy_ignore.txt'"

if [ $# -ge 1 ];then 

	if [ $# -ge 2 ];then 
		case "$2" in
		unity_export)  RSYNC_CMD="$RSYNC_CMD --exclude-from 'copy_ignore/unity_export.txt'"
		;;
		esac
	fi
	
	if [ $1 = "WIN" ];then
		SCRIPT_PATH=$(dirname $0) # Otherwise rsync will think it's remote
		RSYNC_CMD="$RSYNC_CMD --chmod=ugo=rwX" # Otherwise permissions are screwed up
	fi
fi

RSYNC_CMD_W_DELETE="$RSYNC_CMD --delete"

###############################################################################

echo RUN ASSETS SYNCING SHELL SCRIPT

source_path=$SCRIPT_PATH/../$3
destination_path=$SCRIPT_PATH/../$4

$RSYNC_CMD_W_DELETE $source_path/* $destination_path

# The wildcard in rsync leads missing deletion in the top-level directory so let's fix that
diff -arq $source_path $destination_path | grep "Only in $destination_path" | cut -d" " -f4 | xargs -I{} rm -r -f $destination_path/{}


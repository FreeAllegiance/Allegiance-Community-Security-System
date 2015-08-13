SET OutputDir=%CD%

pushd "C:\Source\Allegiance\Allegiance\tag\FAZ_R5"

svn diff > %OutputDir%\ACSS.patch

popd

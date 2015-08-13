// Client.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>

int _tmain(int argc, _TCHAR* argv[])
{
	char *target = new char[256];
	target[0] = '\0';

	char *memoryLocation = new char[256];
	int dataLen = sprintf(memoryLocation, "%ld", target);
	memoryLocation[dataLen] = '\0';

	HANDLE hWrite = CreateFile(_T("\\\\.\\pipe\\allegqueue"), 
		FILE_GENERIC_WRITE, 0, NULL, CREATE_NEW, 0, NULL);

	DWORD numWritten;
	WriteFile(hWrite, memoryLocation, dataLen, &numWritten, NULL);
	CloseHandle(hWrite);

	while(strlen(target) == 0)
		Sleep(100);

	printf("Complete: %s...\n", target);

	return 0;
}

// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__D86241DB_1492_11D4_B3AB_006008BD86D1__INCLUDED_)
#define AFX_STDAFX_H__D86241DB_1492_11D4_B3AB_006008BD86D1__INCLUDED_

#define _WIN32_WINNT 0x0601

#if _MSC_VER >= 1000
#pragma once
#pragma comment(linker,"\"/manifestdependency:type='win32' \
name='Microsoft.Windows.Common-Controls' version='6.0.0.0' \
processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif // _MSC_VER >= 1000

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdisp.h>        // MFC OLE automation classes
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT


//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.
//https://blog.csdn.net/xxxbbjjfz/article/details/53874913
#define CMemDC XCMemDC

#endif // !defined(AFX_STDAFX_H__D86241DB_1492_11D4_B3AB_006008BD86D1__INCLUDED_)

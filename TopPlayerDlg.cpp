// TopPlayerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "duallistdemo.h"
#include "TopPlayerDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

// high scores
#define NAMELEN			20
typedef struct HighScoreType {
	TCHAR name[NAMELEN];
	long score;
	int  health;
	TCHAR  fame[20];
} HighScore;

HighScore hscores[11];
#define MAXSTRLEN  30

CString GetFameStr(int fame);

/////////////////////////////////////////////////////////////////////////////
// CTopPlayerDlg dialog


CTopPlayerDlg::CTopPlayerDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CTopPlayerDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTopPlayerDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CTopPlayerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTopPlayerDlg)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	DDX_Control(pDX, IDC_TOP_PLAYER_LIST, m_list1);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CTopPlayerDlg, CDialog)
	//{{AFX_MSG_MAP(CTopPlayerDlg)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTopPlayerDlg message handlers

BOOL CTopPlayerDlg::OnInitDialog()
{
	CDialog::OnInitDialog();
	m_list1.SendMessage(LVM_SETEXTENDEDLISTVIEWSTYLE, LVS_EX_FULLROWSELECT, LVS_EX_FULLROWSELECT);

	// create column
	m_list1.InsertColumn(0, _TEXT("名次"), LVCFMT_LEFT, 68);
	m_list1.InsertColumn(1, _TEXT("姓名"), LVCFMT_LEFT, 100);
	m_list1.InsertColumn(2, _TEXT("金钱"), LVCFMT_LEFT, 120);
	m_list1.InsertColumn(3, _TEXT("健康程度"), LVCFMT_LEFT, 80);
	m_list1.InsertColumn(4, _TEXT("名声"), LVCFMT_LEFT, 80);
	//    LoadSavedScore();
	ShowScores();


	// TODO: Add extra initialization here

	return TRUE;  // return TRUE unless you set the focus to a control
				  // EXCEPTION: OCX Property Pages should return FALSE
}

void CTopPlayerDlg::InitHighScores()
{

	LoadSavedScore();

}


void CTopPlayerDlg::InsertScore(CString playername, long newscore, int health, int fame)
{
	int i = 0, j = 0;
	m_nCurSel = 0;
	while (i < 10)
	{
		if (newscore >= hscores[i].score)
		{

			for (j = 9; j > i; j--)
			{
				hscores[j].score = hscores[j - 1].score;
				_tcscpy(hscores[j].name, hscores[j - 1].name);
				hscores[j].health = hscores[j - 1].health;
				_tcscpy(hscores[j].fame, hscores[j - 1].fame);
			}
			hscores[i].score = newscore;
			_tcscpy(hscores[i].name, playername);
			hscores[j].health = health;
			_tcscpy(hscores[j].fame, GetFameStr(fame));
			m_nCurSel = i;
			i = 20;
		}
		i++;
	}
}

void CTopPlayerDlg::ShowScores()
{
	CString str[10] = { _TEXT("第一名"),	_TEXT("第二名"),	_TEXT("第三名"),
						_TEXT("第四名"),	_TEXT("第五名"),	_TEXT("第六名"),
						_TEXT("第七名"),	_TEXT("第八名"),	_TEXT("第九名"),
						_TEXT("第十名") };
	int i;
	CString tempstr;
	for (i = 0; i < 10; i++) 
	{
		int n = m_list1.InsertItem(0, str[9 - i]);

		m_list1.SetItemText(n, 1, hscores[9 - i].name);
		tempstr.Format(_TEXT("%ld"), hscores[9 - i].score);

		m_list1.SetItemText(n, 2, tempstr);
		tempstr.Format(_TEXT("%d"), hscores[9 - i].health);

		m_list1.SetItemText(n, 3, tempstr);
		//tempstr.Format(_TEXT("%d"),hscores[9-i].fame);
		m_list1.SetItemText(n, 4, hscores[9 - i].fame);
	}
	m_list1.SetItemState(m_nCurSel, LVIS_SELECTED, LVIS_SELECTED);
}

int CTopPlayerDlg::DoModal()
{
	// TODO: Add your specialized code here and/or call the base class

	SaveScore();
	return CDialog::DoModal();
}
//<--
void CTopPlayerDlg::LoadSavedScore()
{
	//char name[35];
	//long score;
	//int health;
	int i;
	FILE* fp;
	char line[31];
	CString str = AfxGetApp()->m_pszHelpFilePath;
	int n = str.ReverseFind('\\');
	CString score_file_path = str.Left(n);
	score_file_path += _TEXT("\\score.txt");
	errno_t err = _tfopen_s(&fp, score_file_path, _TEXT("rb"));
	if (err != 0)   // not find score.txt, create a empty score list
	{
		_tcscpy(hscores[0].name, _TEXT("赖皮张"));
		hscores[0].score = 12500720;
		hscores[0].health = 98;
		_tcscpy(hscores[0].fame, _TEXT("争议人物"));

		_tcscpy(hscores[1].name, _TEXT("萧峰"));
		hscores[1].score = 830050;
		hscores[1].health = 100;
		_tcscpy(hscores[1].fame, _TEXT("杰出青年"));

		_tcscpy(hscores[2].name, _TEXT("二黑"));
		hscores[2].score = 500447;
		hscores[2].health = 78;
		_tcscpy(hscores[2].fame, _TEXT("德高望重"));

		_tcscpy(hscores[3].name, _TEXT("Andy Rocky"));
		hscores[3].score = 239403;
		hscores[3].health = 97;
		_tcscpy(hscores[3].fame, _TEXT("很差"));

		_tcscpy(hscores[4].name, _TEXT("li xing"));
		hscores[4].score = 34900;
		hscores[4].health = 35;
		_tcscpy(hscores[4].fame, _TEXT("江湖唾弃"));

		_tcscpy(hscores[5].name, _TEXT("li xing"));
		hscores[5].score = 13400;
		hscores[5].health = 100;
		_tcscpy(hscores[5].fame, _TEXT("江湖唾弃"));

		_tcscpy(hscores[6].name, _TEXT("li "));
		hscores[6].score = 2300;
		hscores[6].health = 77;
		_tcscpy(hscores[6].fame, _TEXT("不佳"));

		_tcscpy(hscores[7].name, _TEXT("li "));
		hscores[7].score = 45;
		hscores[7].health = 12;
		_tcscpy(hscores[7].fame, _TEXT("杰出青年"));

		_tcscpy(hscores[8].name, _TEXT("li"));
		hscores[8].score = 34;
		hscores[8].health = 100;
		_tcscpy(hscores[8].fame, _TEXT("一般般"));

		_tcscpy(hscores[9].name, _TEXT("li"));
		hscores[9].score = 3;
		hscores[9].health = 100;
		_tcscpy(hscores[9].fame, _TEXT("杰出青年"));

		return;
	}
	fseek(fp, 0L, SEEK_SET);
	TCHAR temp[31];
	for (i = 0; i < 10; i++)
	{
		fgets(line, MAXSTRLEN, fp);
		//sprintf(name,_TEXT("%s"),line);
		for (int j = 0; j < strlen(line); j++)
		{
			if (line[j] == 0x0d) {
				temp[j] = '\0';
				break;
			}
			temp[j] = line[j];

		}
		_tcscpy(hscores[i].name, temp);

		fgets(line, MAXSTRLEN, fp);
		//sprintf(name,_TEXT("%ld"),);
		hscores[i].score = atol(line);

		fgets(line, MAXSTRLEN, fp);
		hscores[i].health = atoi(line);
		//sprintf(name,_TEXT("%s"),line);
		fgets(line, MAXSTRLEN, fp);
		for (int j = 0; j < strlen(line); j++)
		{
			if (line[j] == 0x0d) {
				temp[j] = '\0';
				break;
			}
			temp[j] = line[j];

		}
		_tcscpy(hscores[i].fame, temp);


	}
	fclose(fp);
}

void CTopPlayerDlg::SaveScore()
{
	int i;
	FILE* fp;
	TCHAR  temp[40];
	CString str = AfxGetApp()->m_pszHelpFilePath;
	int n = str.ReverseFind('\\');
	CString str1 = str.Left(n);
	str1 += _TEXT("\\score.txt");
	//fp = _tfopen(str1, _TEXT("w"));
	errno_t err = _tfopen_s(&fp, str1, _TEXT("w"));
	if (err != 0)
	{
		AfxMessageBox(_TEXT("Error: score file cannot be opened."));
		exit(-1);
	}
	for (i = 0; i < 10; i++)
	{
		_tcscpy(temp, hscores[i].name);
		_ftprintf(fp, _TEXT("%s\n"), temp);
		_ftprintf(fp, _TEXT("%ld\n"), hscores[i].score);
		_ftprintf(fp, _TEXT("%d\n"), hscores[i].health);
		_ftprintf(fp, _TEXT("%s\n"), hscores[i].fame);
	}
	fclose(fp);
}

CString GetFameStr(int fame)
{
	if (fame >= 100)
		return _TEXT("德高望重");
	else if (fame < 100 && fame >= 90)
		return _TEXT("杰出青年");
	else if (fame < 90 && fame >= 80)
		return _TEXT("一般般");
	else if (fame < 80 && fame >= 60)
		return _TEXT("不佳");
	else if (fame < 60 && fame >= 40)
		return _TEXT("争议人物");
	else if (fame < 40 && fame >= 20)
		return _TEXT("差");
	else if (fame < 20 && fame >= 10)
		return _TEXT("很差");
	else if (fame < 10)
		return _TEXT("江湖唾弃");
	else
		return _TEXT("江湖唾弃");


}

// decide my order in the list
// returns:
//            1: if number 1
//          100: if not in the order
int CTopPlayerDlg::GetMyOrder(long score)
{
	int i;
	for (i = 0; i < 10; i++) {
		if (score >= hscores[i].score)
			break;
	}
	if (i >= 10)
		return 100;
	else
		return i;

}

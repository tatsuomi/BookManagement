using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Renci.SshNet;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // SSH用接続情報
        private static readonly string host = "131.206.20.96";       // ホスト名
        private static readonly int port = 22;                       // ポート番号
        private static readonly string SSH_user = "bmdb";            // ホストのユーザ名
        private static readonly string SSH_pass = "bachelor";        // ホストのPW

        // MySQL用接続情報
        private static readonly string server = "localhost";
        private static readonly string database = "bookmanagement";  // データベース名
        private static readonly string MySQL_user = "root";          // ユーザ名
        private static readonly string MySQL_pass = "p4P_ranD";      // パスワード
        private static readonly string charset = "utf8";             // 文字コード

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                 * 
                 * ssh接続
                 * 
                 */

                // コネクション情報
                ConnectionInfo info = new ConnectionInfo(host, port, SSH_user,
                    new AuthenticationMethod[] {
                        new PasswordAuthenticationMethod(SSH_user, SSH_pass)
                        /* PrivateKeyAuthenticationMethod("キーの場所")を指定することでssh-key認証にも対応しています */
                    }
                );
                // クライアント作成
                SshClient ssh = new SshClient(info);
                // 接続開始
                ssh.Connect();

                if (ssh.IsConnected)
                {
                    // 接続に成功した（接続状態である）
                    Console.WriteLine("[OK] SSH Connection succeeded!!");
                }
                else
                {
                    // 接続に失敗した（未接続状態である）
                    Console.WriteLine("[NG] SSH Connection failed!!");
                    return;
                }

                /*
                 * 
                 * db接続
                 * 
                 */

                string connectionString = string.Format("Server ={0}; Database ={1}; Uid ={2}; Pwd ={3}; Charset ={4}", server, database, MySQL_user, MySQL_pass, charset);
                MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();	// 接続
                Console.WriteLine("MySQLに接続しました！");
                connection.Close();	// 接続の解除

                // 接続終了
                ssh.Disconnect();
            }
            catch (Exception ex)
            {
                // エラー発生時
                Console.WriteLine(ex);
                throw ex;
            }
        }
    }
}

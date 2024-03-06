using System.IO;

namespace WinFormsExplorerApp
{
	public partial class ExplorerForm : Form
	{
		Dictionary<string, FileType> fileExtensions;
		private bool sortByAlphavet = true;

		public ExplorerForm()
		{
			InitializeComponent();

			fileExtensions = new()
			{
				{ "docx", FileType.Word  },
				{ "xclx", FileType.Excel },
				{ "pdf", FileType.Pdf },
				{ "jpg", FileType.Jpg },
				{ "png", FileType.Png },
				{ "txt", FileType.Text },
			};

			FillDrives();
		}

		private void treeDirs_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			e.Node?.Nodes.Clear();
			try
			{
				if (Directory.Exists(e.Node?.FullPath))
				{
					string[] dirs = Directory.GetDirectories(e.Node.FullPath)
											.OrderByDescending(node => node)
											.ToArray();

					if (sortByAlphavet)
					{
						Array.Sort(dirs);
					}

					foreach (string dir in dirs)
					{
						TreeNode dirNode = new TreeNode(new DirectoryInfo(dir).Name);
						FillDirNode(dirNode, dir);
						e.Node.Nodes.Add(dirNode);
					}
					FillDirList(e.Node?.FullPath);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		private void treeDirs_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			e.Node?.Nodes.Clear();
			try
			{
				if (Directory.Exists(e.Node?.FullPath))
				{
					string[] dirs = Directory.GetDirectories(e.Node.FullPath)
											 .OrderByDescending(node => node)
											 .ToArray();

					if (sortByAlphavet)
					{
						Array.Sort(dirs);
					}

					foreach (string dir in dirs)
					{
						TreeNode dirNode = new TreeNode(new DirectoryInfo(dir).Name);
						FillDirNode(dirNode, dir);
						e.Node.Nodes.Add(dirNode);
					}
					FillDirList(e.Node?.FullPath);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		private void FillDrives()
		{
			try
			{
				foreach (DriveInfo drive in DriveInfo.GetDrives())
				{
					if (drive.IsReady)
					{
						TreeNode driveNode = new TreeNode() { Text = drive.Name };
						FillDirNode(driveNode, drive.Name);
						treeDirs.Nodes.Add(driveNode);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FillDirNode(TreeNode parentNode, string path)
		{
			try
			{
				string[] dirs = Directory.GetDirectories(path)
										 .OrderByDescending(node => node)
										 .ToArray();

				if (sortByAlphavet)
				{
					Array.Sort(dirs);
				};

				foreach (string dir in dirs)
				{
					TreeNode dirNode = new();
					dirNode.Text = dir.Remove(0, dir.LastIndexOf(@"\") + 1);
					parentNode.Nodes.Add(dirNode);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		private void FillDirList(string path)
		{
			listDir.Items.Clear();

			string[] dirs = Directory.GetDirectories(path)
									 .OrderByDescending(node => node)
									 .ToArray();

			if (sortByAlphavet)
			{
				Array.Sort(dirs);
			};

			foreach (var dir in dirs)
			{
				ListViewItem item = new ListViewItem();
				item.Text = dir.Remove(0, dir.LastIndexOf(@"\") + 1);
				item.ImageIndex = 0;
				listDir.Items.Add(item);

				item.SubItems.Add("<dir>");
			}

			string[] files = Directory.GetFiles(path)
									  .OrderByDescending(file => file)
									  .ToArray();

			if (sortByAlphavet)
			{
				Array.Sort(files);
			};

			foreach (var file in files)
			{
				ListViewItem item = new ListViewItem();
				item.Text = file.Remove(0, file.LastIndexOf(@"\") + 1);

				string ext = file.Remove(0, file.LastIndexOf(".") + 1);

				if (fileExtensions.ContainsKey(ext))
					item.ImageIndex = (int)fileExtensions[ext];
				else
					item.ImageIndex = 7;

				var fileInfo = new FileInfo(file);
				item.SubItems.Add(fileInfo.Length.ToString());

				listDir.Items.Add(item);
			}

		}

		private void btnLarge_Click(object sender, EventArgs e)
		{
			listDir.View = View.LargeIcon;
		}

		private void btnSmall_Click(object sender, EventArgs e)
		{
			listDir.View = View.SmallIcon;
		}

		private void btnList_Click(object sender, EventArgs e)
		{
			listDir.View = View.List;
		}

		private void btnDetails_Click(object sender, EventArgs e)
		{
			listDir.View = View.Details;
		}

		private void sortButton_Click(object sender, EventArgs e)
		{
			sortByAlphavet = !sortByAlphavet;
			sortButton.Text = sortByAlphavet ? "Sorted by Alphavet" : "Sorted by Alphavet (Descending)";

			treeDirs.SelectedNode.Nodes.Clear();
			FillDirNode(treeDirs.SelectedNode, treeDirs.SelectedNode.FullPath);

			listDir.Items.Clear();
			FillDirList(treeDirs.SelectedNode.FullPath);
		}
	}
}
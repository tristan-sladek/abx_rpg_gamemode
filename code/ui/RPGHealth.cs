using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

[Library]
public partial class RPGHealth : Panel
{
	public static RPGHealth Instance;
	public Label HP;
	public Panel HPBack;
	public Panel HPFore;
	public float Health { get; set; }
	public RPGHealth()
	{
		Instance = this;
		StyleSheet.Load( "/ui/RPGHud.scss" );
		HPBack = Add.Panel( "HPBack" );
		HPFore = HPBack.Add.Panel( "HPFore" );
		Add.Panel( "HPBorder" ); // hacky standin
		Add.Label( "HP", "HPText" );
		HP = Add.Label( "100", "HPValue" );
		
		
	}

	public override void Tick()
	{
		base.Tick();
		float size = 196; //magic number, size of element's width

		Health = ((Time.Now * 2) % 100);
		HP.Text = Health.CeilToInt().ToString();
		HPFore.Style.Width = Health * ( size / 100 );
		HPFore.Style.Dirty();
	}
}

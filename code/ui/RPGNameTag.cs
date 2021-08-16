using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.UI
{
	public class BaseRPGNameTag : Panel
	{
		public Label Name;
		public Panel HPFore;
		BaseActor actor;

		public BaseRPGNameTag( BaseActor actor )
		{
			this.actor = actor;
			Name = Add.Label( $"{actor.ActorName}" );
			var HPBack = Add.Panel( "HPBack" );
			HPFore = HPBack.Add.Panel( "HPFore" );

		}
		public virtual void UpdateFromActor( BaseActor actor )
		{
			// Nothing to do unless we're showing health and shit
		}
	}

	public class RPGNameTag : Panel
	{
		Dictionary<BaseActor, BaseRPGNameTag> ActiveTags = new Dictionary<BaseActor, BaseRPGNameTag>();

		public float MaxDrawDistance = 400;
		public int MaxTagsToShow = 5;

		public RPGNameTag()
		{
			StyleSheet.Load( "/ui/RPGNameTags.scss" );
		}

		public override void Tick()
		{
			base.Tick();


			var deleteList = new List<BaseActor>();
			deleteList.AddRange( ActiveTags.Keys );

			int count = 0;
			foreach ( var actor in Entity.All.OfType<BaseActor>().OrderBy( x => Vector3.DistanceBetween( x.Position, CurrentView.Position ) ) )
			{
				if ( UpdateNameTag( actor ) )
				{
					deleteList.Remove( actor );
					count++;
				}

				if ( count >= MaxTagsToShow )
					break;
			}

			foreach( var actor in deleteList )
			{
				ActiveTags[actor].Delete();
				ActiveTags.Remove( actor );
			}

		}

		public virtual BaseRPGNameTag CreateNameTag( BaseActor actor )
		{
			if ( actor == null )
				return null;

			var tag = new BaseRPGNameTag( actor );
			tag.Parent = this;
			return tag;
		}

		public bool UpdateNameTag( BaseActor actor )
		{
			// Don't draw local player
			//if ( actor == Local.Pawn as BaseActor )
			//	return false;

			//if ( player.LifeState != LifeState.Alive )
			//	return false;

			//
			// Where we putting the label, in world coords
			//
			//var head = actor.GetAttachment( "hat" ) ?? new Transform( actor.EyePos );
			//var labelPos = head.Position + head.Rotation.Up * 5;
			var labelPos = actor.EyePos + Vector3.Up * 15;

			//
			// Are we too far away?
			//
			float dist = labelPos.Distance( CurrentView.Position );
			if ( dist > MaxDrawDistance )
				return false;

			//
			// Are we looking in this direction?
			//
			var lookDir = (labelPos - CurrentView.Position).Normal;
			if ( CurrentView.Rotation.Forward.Dot( lookDir ) < 0.5 )
				return false;

			// TODO - can we see them


			MaxDrawDistance = 400;

			// Max Draw Distance


			var alpha = dist.LerpInverse( MaxDrawDistance, MaxDrawDistance * 0.1f, true );

			

			if ( !ActiveTags.TryGetValue( actor, out var tag ) )
			{
				tag = CreateNameTag( actor );
				if ( tag != null )
				{
					ActiveTags[actor] = tag;
				}
			}
			tag.UpdateFromActor( actor );
			tag.Name.Text = actor.ActorName + " " + actor.HP.ToString();

			var screenPos = labelPos.ToScreen();

			tag.Style.Left = Length.Fraction( screenPos.x );
			tag.Style.Top = Length.Fraction( screenPos.y );
			tag.Style.Opacity = alpha;

			var transform = new PanelTransform();
			transform.AddTranslateY( Length.Fraction( -1.0f ) );
			transform.AddTranslateX( Length.Fraction( -0.5f ) );

			var objectSize = 0.05f / dist / (2.0f * MathF.Tan( (CurrentView.FieldOfView / 2.0f).DegreeToRadian() )) * 1500.0f;
			objectSize = objectSize.Clamp( 0.05f, 1.0f );
			objectSize *= 2;

			transform.AddScale( objectSize );
			
			tag.Style.Transform = transform;
			tag.HPFore.Style.Width = Length.Fraction( actor.HP / 100 );
			tag.HPFore.Style.Dirty();
			tag.Style.Dirty();

			return true;
		}
	}
}

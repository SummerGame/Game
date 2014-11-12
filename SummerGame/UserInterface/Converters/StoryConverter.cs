using System;
using System.Windows;
using System.Windows.Shapes;
using GameEngine.Characters;
using GameEngine.StoryTelling;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Media;
using UserInterface.Presenters;

namespace UserInterface
{
    public static class StoryConverter
    {

        // http://msdn.microsoft.com/ru-ru/library/bb613579.aspx <-- магия!
        public static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static void AttackAnimation(Shape fig) 
        {
            //fig.Opacity = 1;

            DoubleAnimation xAnimation = new DoubleAnimation(0, 200, new Duration(TimeSpan.FromSeconds(1)));
            DoubleAnimation yAnimation = new DoubleAnimation(200,0, new Duration(TimeSpan.FromSeconds(1)));
            DoubleAnimation oAnimation = new DoubleAnimation(0,1,new Duration(TimeSpan.FromSeconds(0.3)));

            fig.BeginAnimation(Shape.OpacityProperty, oAnimation);
            fig.BeginAnimation(Shape.WidthProperty,xAnimation);
            yAnimation.BeginTime = TimeSpan.FromSeconds(1);
            fig.BeginAnimation(Shape.HeightProperty,yAnimation);
            
        }

        public static Storyboard GetAnimation(Stories stories)
        {
            Storyboard st = new Storyboard();
            var mapPresenter = FindVisualChild<ContentPresenter>(UI.TheMapControl);
            var unitPresenter = (ItemsControl)UI.TheMapControl.ContentTemplate.FindName("UnitsMap", mapPresenter);
            foreach (Story story in stories.GetStories)
            {
                if (story is UnitStory)
                {
                    for (int i = 0; i < unitPresenter.Items.Count; i++)
                    {
                        var unitContainer = (ContentPresenter)unitPresenter.ItemContainerGenerator.ContainerFromIndex(i);
                        var unit = (unitPresenter.Items[i] as UnitPresenter).Unit;
                        if (unit == (story as UnitStory).Unit)
                        {
                            var unitStates = (story as UnitStory).States.ConvertAll(x => x as UnitState);

                            double time = 0;
                            for (int j = 0; j < unitStates.Count; j++)
                            {
                                if (unitStates[j].Action == StateAction.Move && j+1 < unitStates.Count )
                                {
                                    Storyboard states = new Storyboard();

                                    var p1 = Transformer.ConvertToScreen(unitStates[j].Position);
                                    var p2 = Transformer.ConvertToScreen(unitStates[j + 1].Position);

                                    var t1 = unitStates[j + 1].Time;
                                    DoubleAnimation xAnimation = new DoubleAnimation(p1.X, p2.X,
                                                                                     new Duration(
                                                                                         TimeSpan.FromSeconds(t1)));
                                    DoubleAnimation yAnimation = new DoubleAnimation(p1.Y, p2.Y,
                                                                                     new Duration(
                                                                                         TimeSpan.FromSeconds(t1)));
                                    Storyboard stY = new Storyboard();
                                    stY.Children.Add(yAnimation);
                                    Storyboard stX = new Storyboard();
                                    stX.Children.Add(xAnimation);

                                    time += unitStates[j].Time;
                                    xAnimation.BeginTime = TimeSpan.FromSeconds(time);
                                    yAnimation.BeginTime = TimeSpan.FromSeconds(time);

                                    Storyboard.SetTarget(stX, unitContainer);
                                    Storyboard.SetTarget(stY, unitContainer);

                                    Storyboard.SetTargetProperty(stX, new PropertyPath("(Canvas.Left)"));
                                    Storyboard.SetTargetProperty(stY, new PropertyPath("(Canvas.Top)"));

                                    st.Children.Add(stX);
                                    st.Children.Add(stY);

                                }
                                else if (unitStates[j].Action == StateAction.Attack)
                                {
                                    var c = unitContainer.ContentTemplate.FindName("TheCanvas", unitContainer);
                                    var m = (c as Canvas).FindName("CombatMarker");

                                    //AttackAnimation(m as Shape);       
                                    DoubleAnimation oAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
                                    Storyboard cm = new Storyboard();
                                    Storyboard.SetTarget(cm, m as Shape);
                                    Storyboard.SetTargetProperty(cm, new PropertyPath("Opacity"));
                                    cm.Children.Add(oAnimation);
                                    st.Children.Add(cm);
                                    
                                }
                            }
                            
                        }
                    }
                }
            }
            return st;
        }

    }
}

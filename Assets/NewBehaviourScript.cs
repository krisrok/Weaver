using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnityEngine;
using Weaver;

/// <summary>
/// This class produces a warning, see WeaverSettings log. 
/// 
/// I had to pull in the PropertyChanged.Fody sources to degrade an exception to an error so the build passes.
/// See IsChangedMethodFinder.cs, FindIsChangedMethod(), line 99ff:
/// Added the try/catch block and emit warning instead of letting the exception bubble up.
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class CustomObservableCollection<T> : ObservableCollection<T>
{ }

/// <summary>
/// This class should be processed just fine by Weaver and PropertyChanged.Fody
/// </summary>
[ExecuteAlways]
public class NewBehaviourScript : MonoBehaviour, INotifyPropertyChanged
{
	[OnChanged(nameof(OnChangedHandler))]
    public int Foo { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

    [ContextMenu("Inc")]
    public void Inc()
	{
        Foo++;
	}

	void OnEnable()
    {
		PropertyChanged += PropertyChangedHandler;

		Foo++;
    }

	private void OnChangedHandler(int i)
	{
		Debug.Log("OnChanged weaved correctly: " + i);
	}

	private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
	{
		Debug.Log("PropertyChanged.Fody weaved correctly: " + e.PropertyName);
	}
}

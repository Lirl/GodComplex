using System;
using System.Collections;

public class Rule {
    public Predicate<Hashtable> Predicate;
    public RuleHandler OnSuccess;
    public RuleHandler OnFailure;

    // Rule name and description
    public string Name;
    public string Image;
    public string Description;

    // Penalty text to present of card played incorrectly
    public string penaltyMessage;

    public bool important = false;

    public Rule(Predicate<Hashtable> pre, RuleHandler success, RuleHandler failure) {
        Predicate = pre;
        OnSuccess = success;
        OnFailure = failure;
    }
    public Rule(Predicate<Hashtable> pre, RuleHandler success, RuleHandler failure, bool important) {
        Predicate = pre;
        OnSuccess = success;
        OnFailure = failure;
        this.important = important;
    }

    public Rule(Predicate<Hashtable> pre, RuleHandler success, RuleHandler failure, bool important, string name, string description, string imagePath) : this(pre, success, failure) {
        Description = description;
        Name = name;
        Image = imagePath;
    }

    public bool Validate(Hashtable arg) {

        return Predicate(arg);
    }

    public override string ToString() {
        if(Name != null) {
            return Name;
        }
        return "";
    }
}

from typing import Callable

class Character:
    def __init__(self, name):
        self.name = name

    def __str__(self):
        return f"{self.name}"

    def __repr__(self):
        return f"Character with name {self.name}"

# Define characters
harry = Character(name="Harry")
hermione = Character(name="Hermione")
ron = Character(name="Ron")
draco = Character(name="Draco")
voldemort = Character(name="Voldemort")

characters = [harry, hermione, ron, draco, voldemort]

# Hogwarts predicates
is_wizard = "is_wizard"
is_brave = "is_brave"
is_clever = "is_clever"
has_magic_wand = "has_magic_wand"
does_fight_evil = "does_fight_evil"

# Procedures
def ask_question(character: Character, predicate: Callable):
    fact = fact_dictionary[predicate].get(character)
    if fact is True:
        return True
    elif fact is False:
        return False
    else:
        return None

# Rules
def is_good_wizard(character: Character):
    is_wizard_fact = fact_dictionary[is_wizard].get(character)
    is_brave_fact = fact_dictionary[is_brave].get(character)
    is_clever_fact = fact_dictionary[is_clever].get(character)
    has_wand_fact = fact_dictionary[has_magic_wand].get(character)
    does_fight_evil_fact = fact_dictionary[does_fight_evil].get(character)

    if (is_wizard_fact is True and is_brave_fact is True and has_wand_fact is True) \
            or (is_clever_fact is True and is_wizard_fact is True and does_fight_evil_fact is True):
        return True
    else:
        return False

# Facts
fact_dictionary: dict[Callable, dict[Character, bool | None]] = {
    is_wizard: {
        harry: True,
        hermione: True,
        ron: True,
        draco: True,
        voldemort: True
    },
    is_brave: {},
    is_clever: {},
    has_magic_wand: {},
    does_fight_evil: {}
}

if __name__ == '__main__':
    for character in characters:
        print(f'Is {character.name} brave?')
        x = input()
        if x == "yes":
            fact_dictionary[is_brave][character] = True
        elif x == "no":
            fact_dictionary[is_brave][character] = False
        else:
            fact_dictionary[is_brave][character] = None

        print(f'Is {character.name} clever?')
        x = input()
        if x == "yes":
            fact_dictionary[is_clever][character] = True
        elif x == "no":
            fact_dictionary[is_clever][character] = False
        else:
            fact_dictionary[is_clever][character] = None

        print(f'Does {character.name} have a magic wand?')
        x = input()
        if x == "yes":
            fact_dictionary[has_magic_wand][character] = True
        elif x == "no":
            fact_dictionary[has_magic_wand][character] = False
        else:
            fact_dictionary[has_magic_wand][character] = None

        print(f'Does {character.name} fight evil?')
        x = input()
        if x == "yes":
            fact_dictionary[does_fight_evil][character] = True
        elif x == "no":
            fact_dictionary[does_fight_evil][character] = False
        else:
            fact_dictionary[does_fight_evil][character] = None

        is_good = is_good_wizard(character)
        if is_good:
            print(f"{character.name} is a good wizard!")
        else:
            print(f"{character.name} is not a good wizard.")

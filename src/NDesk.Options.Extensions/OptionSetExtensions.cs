namespace NDesk.Options.Extensions
{
    /// <summary>
    /// OptionSetExtensions class.
    /// </summary>
    public static class OptionSetExtensions
    {
        /// <summary>
        /// Adds a Switch to the OptionSet.
        /// </summary>
        /// <typeparam name="TOptionSet">Any derived OptionSet.</typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The Switch associated with the OptionSet.</returns>
        public static Switch AddSwitch<TOptionSet>(this TOptionSet optionSet, string prototype, string description)
            where TOptionSet : OptionSet
        {
            /* Switch and not a flag. Switch implies on or off, enabled or disabled,
             * whereas flag implies combinations, masking. */
            var @switch = new Switch();

            //Which leaves us injecting a hook into the options.
            optionSet.Add(prototype, description, x => @switch.Enabled = !string.IsNullOrEmpty(x));

            //Kinda like having a future, not quite, but real close.
            return @switch;
        }

        /// <summary>
        /// Adds a strongly typed Variable to the OptionSet.
        /// </summary>
        /// <typeparam name="TVariable"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The Variable associated with the OptionSet.</returns>
        public static Variable<TVariable> AddVariable<TVariable>(this OptionSet optionSet,
            string prototype, string description = null)
        {
            var variablePrototype = prototype + "=";
            var variable = new Variable<TVariable>(variablePrototype);
            optionSet.Add(variablePrototype, description ?? string.Empty,
                x => variable.Value = Variable<TVariable>.CastString(x));
            return variable;
        }

        /// <summary>
        /// Accumulates option values in a strongly-typed Variable list.
        /// </summary>
        /// <typeparam name="TVariable"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <returns>The VariableList associated with the OptionSet.</returns>
        public static VariableList<TVariable> AddVariableList<TVariable>(this OptionSet optionSet,
            string prototype, string description = null)
        {
            var variablePrototype = prototype + "=";
            var variable = new VariableList<TVariable>(variablePrototype);
            optionSet.Add(variablePrototype, description ?? string.Empty, x =>
            {
// ReSharper disable InconsistentNaming
                var x_Value = Variable<TVariable>.CastString(x);
// ReSharper restore InconsistentNaming
                variable.ValuesList.Add(x_Value);
            });
            return variable;
        }

        /// <summary>
        /// Accumulates options into a strongly-typed VariableMatrix.
        /// </summary>
        /// <typeparam name="TVariable"></typeparam>
        /// <param name="optionSet"></param>
        /// <param name="prototype"></param>
        /// <param name="description"></param>
        /// <returns>The VariableMatrix associated with the OptionSet.</returns>
        public static VariableMatrix<TVariable> AddVariableMatrix<TVariable>(this OptionSet optionSet,
            string prototype, string description = null)
        {
            var variablePrototype = prototype + ":";

            var variable = new VariableMatrix<TVariable>(variablePrototype);

            optionSet.Add(variablePrototype, description ?? string.Empty, (k, x) =>
                {
                    if (string.IsNullOrEmpty(k))
                        throw new OptionException("Name not specified", variablePrototype);

// ReSharper disable InconsistentNaming
                    var x_Value = Variable<TVariable>.CastString(x);
// ReSharper restore InconsistentNaming

                    variable.Matrix.Add(k, x_Value);
                });

            return variable;
        }

    }
}
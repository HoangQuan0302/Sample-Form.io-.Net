
let saveForm;
function CreateSimpleForm(formDesign, uiRules) {
    console.log(JSON.parse(uiRules))
    Formio.builder(document.getElementById('builder'),
        {
            components: JSON.parse(formDesign)
        },
        {
            builder: {
                basic: false,
                advanced: false,
                data: false,
                premium: false,
                layout: {
                    components:[]
                },
                custom: {
                    title: 'Field Property',
                    weight: 10,
                    components: JSON.parse(uiRules)
                },
            },
            editForm: {
                textfield: [
                    {
                        key: 'display',
                        ignore: false,
                        components: [
                            {
                                key: "description",
                                ignore: true
                            },
                            {
                                key: 'prefix',
                                ignore: true
                            },
                            {
                                key: 'suffix',
                                ignore: true
                            },
                            {
                                key: 'allowMultipleMasks',
                                ignore: true
                            },
                            {
                                key: 'tabindex',
                                ignore: true
                            },
                            {
                                key: 'autocomplete',
                                ignore: true
                            },
                            {
                                key: 'inputMask',
                                ignore: true
                            },
                            {
                                key: 'displayMask',
                                ignore: true
                            },
                            {
                                key: 'inputMaskPlaceholderChar',
                                ignore: true
                            }
                        ]
                    },
                    {
                        key: 'data',
                        ignore: false,
                        components: [
                            {
                                key: 'multiple',
                                ignore: true
                            },
                            {
                                key: 'defaultValue',
                                ignore: true
                            },
                            {
                                key: 'persistent',
                                ignore: true
                            },
                            {
                                key: 'inputFormat',
                                ignore: true
                            },
                            {
                                key: 'protected',
                                ignore: true
                            },
                            {
                                key: 'dbIndex',
                                ignore: true
                            },
                            {
                                key: 'truncateMultipleSpaces',
                                ignore: true
                            },
                            {
                                key: 'encrypted',
                                ignore: true
                            },
                            {
                                key: 'redrawOn',
                                ignore: true
                            },
                            {
                                key: 'calculateServer',
                                ignore: true
                            },
                            {
                                key: 'allowCalculateOverride',
                                ignore: true
                            },
                            {
                                key: 'customDefaultValue',
                                ignore: true
                            }
                        ]
                    },
                    {
                        key: 'validation',
                        ignore: true,
                    },
                    {
                        key: 'api',
                        ignore: true
                    },
                    {
                        key: 'conditional',
                        ignore: true
                    },
                    {
                        key: 'layout',
                        ignore: true
                    },
                    {
                        key: 'logic',
                        ignore: true
                    },
                    {
                        key: 'condition',
                        ignore: true
                    }
                ]
            }
        }).then(function (builder) {
            builder.on('saveComponent', function () {
                saveForm = builder.schema;
            });
        });
}

function formBuilderDefault(uiRules) {
    Formio.builder(document.getElementById('builder'), {}, {
        builder: {
            basic: false,
            advanced: false,
            data: false,
            premium: false,
            layout: {
                components:[]
            },
            custom: {
                title: 'Field Property',
                weight: 10,
                components: JSON.parse(uiRules)
            },
        },
        editForm: {
            textfield: [
                {
                    key: 'display',
                    ignore: false,
                    components: [
                        {
                            key: "description",
                            ignore: true
                        },
                        {
                            key: 'tooltip',
                            ignore: true,
                        },
                        {
                            key: 'prefix',
                            ignore: true
                        },
                        {
                            key: 'suffix',
                            ignore: true
                        },
                        {
                            key: 'allowMultipleMasks',
                            ignore: true
                        },
                        {
                            key: 'customClass',
                            ignore: true
                        },
                        {
                            key: 'tabindex',
                            ignore: true
                        },
                        {
                            key: 'autocomplete',
                            ignore: true
                        },
                        {
                            key: 'inputMask',
                            ignore: true
                        },
                        {
                            key: 'displayMask',
                            ignore: true
                        },
                        {
                            key: 'inputMaskPlaceholderChar',
                            ignore: true
                        }
                    ]
                },
                {
                    key: 'data',
                    ignore: false,
                    components: [
                        {
                            key: 'multiple',
                            ignore: true
                        },
                        {
                            key: 'defaultValue',
                            ignore: true
                        },
                        {
                            key: 'persistent',
                            ignore: true
                        },
                        {
                            key: 'inputFormat',
                            ignore: true
                        },
                        {
                            key: 'protected',
                            ignore: true
                        },
                        {
                            key: 'dbIndex',
                            ignore: true
                        },
                        {
                            key: 'truncateMultipleSpaces',
                            ignore: true
                        },
                        {
                            key: 'encrypted',
                            ignore: true
                        },
                        {
                            key: 'redrawOn',
                            ignore: true
                        },
                        {
                            key: 'calculateServer',
                            ignore: true
                        },
                        {
                            key: 'allowCalculateOverride',
                            ignore: true
                        },
                        {
                            key: 'customDefaultValue',
                            ignore: true
                        }
                    ]
                },
                {
                    key: 'api',
                    ignore: true
                },
                {
                    key: 'conditional',
                    ignore: true
                },
                {
                    key: 'layout',
                    ignore: true
                },
                {
                    key: 'logic',
                    ignore: true
                },
                {
                    key: 'condition',
                    ignore: true
                },
                {
                    key: 'validation',
                    ignore: true
                }
            ]
        }
    }).then(function (builder) {
        builder.on('saveComponent', function () {
            saveForm = builder.schema;
        });
    });
}

function SaveForm() {
    return JSON.stringify(saveForm);
}

function renderForm(formDesign) {
    Formio.icons = 'fontawesome';
    Formio.createForm(document.getElementById('formio'), {
        components: JSON.parse(formDesign)
    }).then(function (form) {
        form.on('submit', function (submission) {
            console.log(submission);
        });
    });
}

function renderInfoList() {
    Formio.createForm(document.getElementById('formio'), {
        components: [
            {
                label: 'Children',
                key: 'children',
                type: 'datagrid',
                input: true,
                validate: {
                    minLength: 3,
                    maxLength: 6
                },
                components: [
                    {
                        label: 'First Name',
                        key: 'firstName',
                        type: 'textfield',
                        input: true
                    },
                    {
                        label: 'Last Name',
                        key: 'lastName',
                        type: 'textfield',
                        input: true
                    },
                    {
                        label: 'Gender',
                        key: 'gender',
                        type: 'select',
                        input: true,
                        data: {
                            values: [
                                {
                                    value: 'male',
                                    label: 'Male'
                                },
                                {
                                    value: 'female',
                                    label: 'Female'
                                },
                                {
                                    value: 'other',
                                    label: 'Other'
                                }
                            ]
                        },
                        dataSrc: 'values',
                        template: '<span>{{ item.label }}</span>'
                    },
                    {
                        type: 'checkbox',
                        label: 'Dependant',
                        key: 'dependant',
                        inputType: 'checkbox',
                        input: true
                    },
                    {
                        label: 'Birthdate',
                        key: 'birthdate',
                        type: 'datetime',
                        input: true,
                        format: 'yyyy-MM-dd hh:mm a',
                        enableDate: true,
                        enableTime: true,
                        defaultDate: '',
                        datepickerMode: 'day',
                        datePicker: {
                            showWeeks: true,
                            startingDay: 0,
                            initDate: '',
                            minMode: 'day',
                            maxMode: 'year',
                            yearRows: 4,
                            yearColumns: 5,
                            datepickerMode: 'day'
                        },
                        timePicker: {
                            hourStep: 1,
                            minuteStep: 1,
                            showMeridian: true,
                            readonlyInput: false,
                            mousewheel: true,
                            arrowkeys: true
                        },
                        "conditional": {
                            "eq": "true",
                            "when": "dependant",
                            "show": "true"
                        }
                    }
                ]
            }
        ]
    }).then(function (form) {
        // Provide a default submission.
        form.submission = {
            data: {
                children: [
                    {
                        firstName: 'Joe',
                        lastName: 'Smith',
                        gender: 'male',
                        dependant: true,
                        birthdate: '1982-05-18'
                    },
                    {
                        firstName: 'Mary',
                        lastName: 'Smith',
                        gender: 'female',
                        dependant: false,
                        birthdate: '1979-02-17'
                    }
                ]
            }
        };
    });
}

function folkHtmlContent(Id) {
    return "";
}
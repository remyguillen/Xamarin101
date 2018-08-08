﻿using Prism.Commands;
using System.Collections.ObjectModel;
using EmpList.Interfaces;
using EmpList.Models;
using Prism.Navigation;

namespace EmpList.ViewModels
{
    public class EmployeeListPageViewModel : ViewModelBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public EmployeeListPageViewModel(INavigationService navigationService, IEmployeeService employeeService) : base(navigationService)
        {
            _employeeService = employeeService;
            _navigationService = navigationService;

            Title = "Employee List";
            Employees = new ObservableCollection<Employee>();

            GetEmployeesFromApi();

            NavigateCommand = new DelegateCommand(Navigate);
        }

        async void Navigate()
        {
            var navigationParams = new NavigationParameters {{"model", _selectedEmployee}};
            await _navigationService.NavigateAsync("EmployeeDetaiilsPage", navigationParams);
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        public DelegateCommand NavigateCommand { get; private set; }

        private async void GetEmployeesFromApi()
        {
            IsRunning = true;
            var result = await _employeeService.GetAllEmployees();
            IsRunning = false;

            foreach (var item in result)
            {
                Employees.Add(item);
            }
        }
    }
}
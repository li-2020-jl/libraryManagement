// Library Management System - Interactive Effects
// Modern UI animations and interactions

document.addEventListener('DOMContentLoaded', function() {
    
    // ===== BUTTON RIPPLE EFFECT =====
    function createRipple(event) {
        const button = event.currentTarget;
        
        // Don't add ripple to disabled buttons
        if (button.disabled) return;
        
        // Create ripple element
        const ripple = document.createElement('span');
        const diameter = Math.max(button.clientWidth, button.clientHeight);
        const radius = diameter / 2;
        
        // Position ripple
        const rect = button.getBoundingClientRect();
        ripple.style.width = ripple.style.height = `${diameter}px`;
        ripple.style.left = `${event.clientX - rect.left - radius}px`;
        ripple.style.top = `${event.clientY - rect.top - radius}px`;
        ripple.classList.add('ripple');
        
        // Remove existing ripples
        const existingRipple = button.getElementsByClassName('ripple')[0];
        if (existingRipple) {
            existingRipple.remove();
        }
        
        button.appendChild(ripple);
        
        // Remove ripple after animation
        setTimeout(() => ripple.remove(), 600);
    }
    
    // Add ripple effect to all buttons
    const buttons = document.querySelectorAll('.btn, button');
    buttons.forEach(button => {
        button.addEventListener('click', createRipple);
    });
    
    
    // ===== FADE-IN ANIMATION FOR PAGE ELEMENTS =====
    const fadeInElements = document.querySelectorAll('h1, h2, .card, .markdown-table-container, form');
    
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry, index) => {
            if (entry.isIntersecting) {
                setTimeout(() => {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }, index * 50); // Stagger animation
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);
    
    fadeInElements.forEach(element => {
        element.style.opacity = '0';
        element.style.transform = 'translateY(20px)';
        element.style.transition = 'opacity 0.6s cubic-bezier(0.4, 0, 0.2, 1), transform 0.6s cubic-bezier(0.4, 0, 0.2, 1)';
        observer.observe(element);
    });
    
    
    // ===== STICKY HEADER & SEARCH BAR (DISABLED - Causes content overlap) =====
    // Sticky functionality disabled to prevent content overlap issues
    // Can be re-enabled with proper spacing adjustments if needed
    
    
    // ===== SMOOTH SCROLL FOR ANCHOR LINKS =====
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
    
    
    // ===== INPUT FOCUS ANIMATION =====
    const inputs = document.querySelectorAll('input[type="text"], input[type="email"], input[type="password"], input[type="tel"], select, textarea');
    inputs.forEach(input => {
        input.addEventListener('focus', function() {
            this.style.transform = 'scale(1.02)';
            this.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
        });
        
        input.addEventListener('blur', function() {
            this.style.transform = 'scale(1)';
        });
    });
    
    
    // ===== CARD HOVER EFFECT =====
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-4px)';
            this.style.boxShadow = '0 8px 20px rgba(0, 0, 0, 0.3)';
            this.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '0 2px 4px rgba(0,0,0,0.1)';
        });
    });
    
    
    // ===== STATUS BADGE ANIMATION =====
    const statusBadges = document.querySelectorAll('.status-available, .status-checked-out');
    statusBadges.forEach(badge => {
        badge.style.animation = 'fadeInUp 0.5s cubic-bezier(0.4, 0, 0.2, 1)';
    });
    
    
    // ===== FORM VALIDATION SHAKE =====
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const invalidInputs = this.querySelectorAll(':invalid');
            if (invalidInputs.length > 0) {
                invalidInputs.forEach(input => {
                    input.classList.add('shake');
                    setTimeout(() => input.classList.remove('shake'), 500);
                });
            }
        });
    });
    
});
